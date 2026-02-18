# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Web platform and desktop app for sharing custom aircraft profiles for the Simionic G1000 avionics iPad apps. Live at https://g1000profiledb.com. Three solutions, three tiers:

- **Next.js frontend** (`src/CustomProfileDB/Simionic.CustomProfiles.Next/`) — TypeScript/Tailwind SPA replacing the Blazor app (Stage 1 migration)
- **Blazor WebAssembly frontend** (`src/CustomProfileDB/Simionic.CustomProfiles.Web/`) — legacy client-side SPA (being replaced by Next.js)
- **Azure Functions API** (`src/CustomProfileDB/Simionic.CustomProfiles.FunctionApp/`) — serverless REST backend with Cosmos DB
- **Windows Forms desktop app** (`src/CustomProfileManager/Simionic.CustomProfiles.DesktopApp/`) — manages profiles on connected iPads via USB

Shared libraries live in `src/Common/`:
- `Simionic.Core` — domain models (Profile, Gauge, AircraftType, VSpeeds, etc.)
- `Simionic.CustomProfiles.Editor` — Blazor Razor components for profile editing (shared between web and desktop)
- `Simionic.CustomProfiles.ImportExport` — SQLite-based profile import/export from iPad app databases

## Build & Run Commands

```bash
# Next.js frontend (new)
cd src/CustomProfileDB/Simionic.CustomProfiles.Next
npm install
npm run dev     # Dev server on localhost:3000
npm run build   # Production build
npm start       # Production server

# Web frontend (Blazor WASM, legacy)
dotnet run --project src/CustomProfileDB/Simionic.CustomProfiles.Web

# Function App (API backend)
dotnet build src/CustomProfileDB/Simionic.CustomProfiles.FunctionApp --configuration Release

# Desktop app (Windows Forms, requires Windows)
dotnet build src/CustomProfileManager/CustomProfileManager.sln

# Restore all packages for a solution
dotnet restore src/CustomProfileDB/CustomProfileDB.sln
dotnet restore src/Common/Simionic.Common.sln
dotnet restore src/CustomProfileManager/CustomProfileManager.sln
```

No test projects exist in this repository.

## Target Frameworks

- Most projects target **net8.0** (Web, FunctionApp, Core, Editor)
- Desktop app targets **net10.0-windows**
- ImportExport library targets **net10.0**

## Architecture Details

### Authentication
Azure AD B2C via MSAL with Microsoft personal accounts (`login.microsoftonline.com/consumers`). The web app requires authentication by default (fallback policy = RequireAuthenticatedUser). `CustomAccountFactory` enriches claims from Microsoft Graph.

### API Layer (Legacy — Azure Functions)
Four Azure Functions (isolated worker model, v4) in `src/CustomProfileDB/Simionic.CustomProfiles.FunctionApp/`. These are being replaced by the Next.js BFF but remain in the repo for reference:
- `GET /api/profile/{profileId}` — single profile with Cosmos DB input binding
- `GET /api/profiles` — list profiles (query param filtering)
- `POST /api/upsert/{profileId}` — create or update profile
- `GET /api/ownerId` — resolve user identity via PBKDF2 hash

### Data Model
`Profile` is the central entity. It contains 13 typed `Gauge` objects (CHT, EGT, RPM, FuelFlow, etc.), each with colored ranges. `ProfileSummary` is a lightweight projection for list views. Profiles have an `AircraftType` enum (Piston, Turboprop, Jet) that determines which gauges are relevant. Profiles support forking (`ForkedFrom` field) and publish/draft status.

### Next.js Frontend & BFF (New)
App Router with TypeScript and Tailwind CSS. Uses NextAuth.js (v4) with Microsoft Entra ID (Azure AD) for authentication. The Node.js BFF layer (Next.js API routes in `src/app/api/`) implements the full backend:
- **Data store** (`src/lib/data-store.ts`) — reads/writes profiles as JSON files in the `data/` directory (one file per profile, filename = GUID). Will be migrated to MongoDB.
- **Owner ID** (`src/lib/owner-id.ts`) — PBKDF2 hash matching the C# implementation (SHA-1, 100k iterations, 24 bytes) to maintain compatibility with existing owner IDs.
- API routes: `GET /api/profiles`, `GET/POST /api/profiles/[id]`, `GET /api/auth/owner-id`
- Configuration via `.env.local` (see `.env.local.example`).

### Blazor Web Frontend (Legacy)
`ProfileStore` is a **static class** (not DI-registered) that handles all API communication. Pages use it directly. The Blazor app is configured in `Program.cs` with MSAL auth wired up. No HttpClient is registered through DI for general use — API calls go through `ProfileStore`'s static methods.

### Desktop App
Uses `iMobileDevice-net` NuGet package to communicate with iPads over USB. `iPadBrowser` and `iPadFileManager` handle device interaction. The ImportExport library reads/writes Simionic's SQLite databases on the device.

## CI/CD

Two GitHub Actions workflows:
- **Azure Static Web Apps** — deploys Blazor frontend on push/PR to main
- **Azure Functions** — deploys API on push to main when `src/CustomProfileDB/Simionic.CustomProfiles.FunctionApp/**` or `src/Common/**` change; uses OIDC auth

## Configuration

- Next.js config: `src/CustomProfileDB/Simionic.CustomProfiles.Next/.env.local` (see `.env.local.example` for required vars)
- Web app config: `src/CustomProfileDB/Simionic.CustomProfiles.Web/wwwroot/appsettings.json`
- Both Web and FunctionApp use .NET User Secrets for local development credentials
- Function App local settings: `local.settings.json` (gitignored)
