# Simionic G1000 Custom Profiles

A Web site and tool for sharing custom aircraft profiles for the [Simionic G1000 apps](https://www.simionic.net/wordpress/g1000-apps/). This project includes a desktop application for managing profiles on iOS devices, and a web-based profile database for community sharing which can be found at [https://g1000profiledb.com](https://g1000profiledb.com).

The Simionic G1000 apps are high-fidelity simulations of the Garmin G1000 avionics system that run on iPad, including Primary Flight Display (PFD) and Multi-Function Display (MFD) units. These apps allow users to create custom aircraft profiles with specific performance characteristics and configurations for unsupported aircraft in the simulator. This Web app and the accompanying Profile Manager desktop app allow users to share and backup their custom profiles.

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

**Technology**: Azure Functions, Cosmos DB

### Custom Profile Manager
A Windows desktop application for managing G1000 custom profiles on your iPad:  
- Import and export profiles from iOS devices
- Edit profile settings and aircraft configurations
- Push profiles directly to connected iPads
- Extract and backup existing profiles

**Technology**: .NET 6.0 Windows Forms

## Getting Started

### Prerequisites
- .NET 6.0 SDK or later (for desktop app)
- .NET 8.0 SDK or later (for function app)
- Visual Studio 2022 or later
- iOS device with Simionic G1000 app installed

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

MIT License

Copyright (c) 2023 Neil Hewitt

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
