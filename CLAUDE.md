# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Web platform and desktop app for sharing custom aircraft profiles for the Simionic G1000 avionics iPad apps. Live at https://g1000profiledb.com. Two solutions:

- **Blazor WebAssembly frontend** (`src/CustomProfileDB/Simionic.CustomProfiles.Web/`) — client-side SPA
- **Azure Functions API** (`src/CustomProfileDB/Simionic.CustomProfiles.FunctionApp/`) — serverless REST backend with Cosmos DB
- **Windows Forms desktop app** (`src/CustomProfileManager/Simionic.CustomProfiles.DesktopApp/`) — manages profiles on connected iPads via USB

Shared libraries live in `src/Common/`:
- `Simionic.Core` — domain models (Profile, Gauge, AircraftType, VSpeeds, etc.)
- `Simionic.CustomProfiles.Editor` — Blazor Razor components for profile editing (shared between web and desktop)
- `Simionic.CustomProfiles.ImportExport` — SQLite-based profile import/export from iPad app databases

## Build & Run Commands

```bash
# Web frontend (Blazor WASM)
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

### Data Model
`Profile` is the central entity. It contains 13 typed `Gauge` objects (CHT, EGT, RPM, FuelFlow, etc.), each with colored ranges (`GaugeRange` with `RangeColour`). `ProfileSummary` is a lightweight projection for list views. Profiles have an `AircraftType` enum (Piston, Turboprop, Jet) that determines which gauges are relevant. Profiles support forking (`ForkedFrom` field) and publish/draft status.

### Owner ID
User identity is a PBKDF2 hash (SHA-1, 100,000 iterations, 24 bytes, fixed base64 salt) producing uppercase hex. Implemented in `Helper.cs` in the FunctionApp. Used as the `Owner.Id` field on profiles to tie them to their creator.

### Authentication
Azure AD B2C via MSAL with Microsoft personal accounts (`login.microsoftonline.com/consumers`). The web app requires authentication by default (fallback policy = RequireAuthenticatedUser). `CustomAccountFactory` enriches claims from Microsoft Graph.

### API Layer (Azure Functions)
Five Azure Functions (isolated worker model, v4) in `src/CustomProfileDB/Simionic.CustomProfiles.FunctionApp/`:
- `GET /api/profile/{profileId}` — single profile
- `GET /api/profiles` — list profiles (query param filtering)
- `GET /api/profilesummaries` — lightweight summaries
- `POST /api/upsert/{profileId}` — create or update profile
- `GET /api/ownerId` — resolve user identity via PBKDF2 hash

### Blazor Web Frontend
`ProfileStore` is a **static class** (not DI-registered) that handles all API communication. Pages use it directly. No HttpClient is registered through DI for general use.

### Desktop App
Uses `iMobileDevice-net` NuGet package to communicate with iPads over USB. `iPadBrowser` lists connected devices, `iPadFileManager` extracts/pushes `ACCustom.db` SQLite databases. The ImportExport library (`CustomProfileDB` class) reads/writes profiles in those databases. On startup, the app checks `g1000profiledb.com/files/simionic-custom-profile-manager-version.txt` for updates.

## CI/CD

Two GitHub Actions workflows:
- **Azure Static Web Apps** (`azure-static-web-apps-blue-beach-00fc4f403.yml`) — deploys Blazor frontend on push/PR to main
- **Azure Functions** (`main_simionic-functions.yml`) — deploys API on push to main when `src/CustomProfileDB/Simionic.CustomProfiles.FunctionApp/**` or `src/Common/**` change; uses OIDC auth

## Configuration

- Web app: `src/CustomProfileDB/Simionic.CustomProfiles.Web/wwwroot/appsettings.json`
- Both Web and FunctionApp use .NET User Secrets for local development credentials
- Function App local settings: `local.settings.json` (checked in with Cosmos DB emulator connection string)
