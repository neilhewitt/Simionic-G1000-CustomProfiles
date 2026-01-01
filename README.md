# Simionic G1000 Custom Profiles

A comprehensive toolset for managing and sharing custom aircraft profiles for the [Simionic G1000 apps](https://www.simionic.net/wordpress/g1000-apps/). This project includes a desktop application for managing profiles on iOS devices and a web-based profile database for community sharing.

## What Are the Simionic G1000 Apps?

The Simionic G1000 apps are high-fidelity simulations of the Garmin G1000 avionics system, featuring Primary Flight Display (PFD) and Multi-Function Display (MFD) units. These apps allow users to create custom aircraft profiles with specific performance characteristics and configurations.

## Project Components

### Profile Database (Web)
A community-driven web platform for discovering and sharing custom aircraft profiles:
- Browse and search available profiles
- Import profiles to your collection
- Create and publish new profiles
- User authentication via Azure AD B2C

**Technology**:  Blazor WebAssembly, Azure Static Web Apps

### Profile Database (Backend)
Azure Functions providing the API layer for profile management:
- RESTful API for profile CRUD operations
- Integration with Azure Cosmos DB
- User identity and ownership management

**Technology**: Azure Functions (. NET 8. 0), Cosmos DB

### Custom Profile Manager
A Windows desktop application for managing G1000 custom profiles on your iPad:  
- Import and export profiles from iOS devices
- Edit profile settings and aircraft configurations
- Push profiles directly to connected iPads
- Extract and backup existing profiles

**Technology**: .NET 6.0 Windows Forms

## Getting Started

### Prerequisites
- .NET 6.0 SDK or later
- Visual Studio 2022 or later (for desktop applications)
- iOS device with Simionic G1000 app installed (for profile management)

### Building the Desktop Manager

```bash
cd src/CustomProfileManager
dotnet restore
dotnet build
```

### Running the Web Application Locally

```bash
cd src/CustomProfileDB/Simionic.CustomProfiles.Web
dotnet run
```

## Project Structure

```
src/
├── Common/                          # Shared libraries
│   ├── Simionic.Core/              # Core data models and types
│   └── Simionic.CustomProfiles.ImportExport/  # Profile import/export logic
├── CustomProfileManager/            # Desktop profile management application
└── CustomProfileDB/                 # Web-based profile database
    ├── Simionic. CustomProfiles.Web/          # Blazor frontend
    └── Simionic.CustomProfiles.FunctionApp/  # Azure Functions API
```

## License

This project is licensed under the MIT license. See LICENSE.md for details.
