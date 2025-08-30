# WUPHF! - The Ultimate Social Networking Experience

Always reference these instructions first and fallback to search or bash commands only when you encounter unexpected information that does not match the info here.

## Working Effectively

### Prerequisites and Environment Setup
- **CRITICAL: Install .NET 10 SDK (Preview)**
  - Download from: https://dotnet.microsoft.com/download/dotnet/10.0
  - Verify installation: `dotnet --version` (should show 10.x.x)
  - The project CANNOT build with earlier .NET versions (.NET 8 or below will fail)

### Bootstrap, Build, and Test the Repository
- **Install MAUI workload for mobile development:**
  ```bash
  dotnet workload install maui
  ```
  - Takes 2-5 minutes depending on connection. NEVER CANCEL. Set timeout to 10+ minutes.

- **For Android development (optional but recommended):**
  - Set environment variables: `ANDROID_SDK_ROOT` and `JAVA_HOME`
  - If not set, Android builds will be skipped (still functional for API and Web)

- **Restore NuGet packages:**
  ```bash
  dotnet restore
  ```
  - Takes 30-60 seconds. NEVER CANCEL. Set timeout to 5+ minutes.

- **Build the entire solution:**
  ```bash
  dotnet build
  ```
  - Takes 2-4 minutes for initial build. NEVER CANCEL. Set timeout to 10+ minutes.
  - Subsequent builds are faster (30-60 seconds)

### Running the Applications

#### 1. Web API (Backend) - ALWAYS START FIRST
```bash
cd src/WUPHF.Api/WUPHF.Api
dotnet run
```
- Available at: `http://localhost:5043` (HTTP) or `https://localhost:7254` (HTTPS)
- OpenAPI documentation: `http://localhost:5043/openapi/v1.json`
- Health check: `http://localhost:5043/api/wuphf/health`
- Takes 10-15 seconds to start. Wait for "Now listening on" message.

#### 2. Blazor Web App (Frontend)
```bash
cd src/WUPHF.Web/WUPHF.Web
dotnet run
```
- Available at: `https://localhost:7277` (HTTPS) or `http://localhost:5047` (HTTP)
- Takes 15-20 seconds to start. Wait for "Now listening on" message.

#### 3. MAUI Mobile App (Cross-platform)
```bash
cd src/WUPHF.Mobile/WUPHF.Mobile
dotnet run --framework net9.0-windows10.0.19041.0  # For Windows
dotnet run --framework net9.0-android              # For Android (requires SDK setup)
```
- Windows: Runs as desktop application
- Android: Requires device/emulator and Android SDK setup
- Takes 30-60 seconds to build and deploy. NEVER CANCEL.

## Validation Requirements

### Manual Testing Scenarios - ALWAYS PERFORM AFTER CHANGES
1. **API Health Check:**
   - Navigate to `http://localhost:5043/api/wuphf/health`
   - Should return JSON with "Status": "WUPHF is alive and kicking!"

2. **Complete WUPHF Flow (Critical):**
   - Start API first, then Web app
   - Navigate to `https://localhost:7277/send`
   - Fill out form: From="Test User", To="Ryan Howard", Message="Test WUPHF!"
   - Select multiple channels (Facebook, Twitter, SMS, Email, Printer)
   - Click "Send WUPHF!"
   - Should show success message with channel count
   - Navigate to `https://localhost:7277/history` to verify message appears

3. **Mobile App Testing (if running):**
   - Launch mobile app
   - Navigate to "Send WUPHF" page
   - Send a test message through multiple channels
   - Verify message appears in history

## Project Structure and Key Locations

### Solution Architecture
```
WUPHF.sln - Main solution file
├── src/WUPHF.Api/          - ASP.NET Core Web API (Backend)
├── src/WUPHF.Web/          - Blazor Server Web App (Frontend)
├── src/WUPHF.Mobile/       - .NET MAUI Mobile App (Cross-platform)
└── src/WUPHF.Shared/       - Shared Models, DTOs, Constants
```

### Frequently Modified Files
- **API Controllers:** `src/WUPHF.Api/Controllers/WuphfController.cs`
- **API Services:** `src/WUPHF.Api/Services/`
- **Web Pages:** `src/WUPHF.Web/Components/Pages/`
- **Mobile Pages:** `src/WUPHF.Mobile/*.xaml` and `*.xaml.cs`
- **Shared Models:** `src/WUPHF.Shared/Models/`
- **Constants/Quotes:** `src/WUPHF.Shared/Constants/`

### Key API Endpoints
- `POST /api/wuphf/send` - Send a WUPHF message
- `GET /api/wuphf/history` - Get message history
- `GET /api/wuphf/{id}` - Get specific message
- `GET /api/wuphf/quotes` - Get Ryan Howard quotes
- `GET /api/wuphf/health` - Health check

## Build Timing and Expectations

### Expected Command Times
- `dotnet workload install maui`: 2-5 minutes (first time only)
- `dotnet restore`: 30-60 seconds
- `dotnet build`: 2-4 minutes (initial), 30-60 seconds (subsequent)
- Application startup: 10-60 seconds depending on project
- **NEVER CANCEL any of these operations - use generous timeouts**

### Timeout Recommendations
- Workload installation: 10+ minutes
- Package restore: 5+ minutes  
- Initial build: 10+ minutes
- Application startup: 3+ minutes
- Subsequent builds: 5+ minutes

## Development Workflow

### Making Changes
1. **Always build and test after changes:**
   ```bash
   dotnet build
   ```

2. **For API changes:**
   - Restart the API service
   - Test endpoints using browser or `curl`
   - Verify OpenAPI documentation updates

3. **For Web UI changes:**
   - Restart the Web app
   - Test in browser with full user scenarios
   - Verify mobile responsiveness

4. **For Mobile changes:**
   - Rebuild and redeploy to target platform
   - Test on actual device when possible

### Common Issues and Solutions
- **Build fails with .NET version error:** Ensure .NET 10 SDK is installed
- **MAUI workload missing:** Run `dotnet workload install maui`
- **Android build fails:** Verify `ANDROID_SDK_ROOT` and `JAVA_HOME` are set
- **API connection errors:** Ensure API is running before starting Web/Mobile apps
- **CORS errors:** API includes permissive CORS policy for development

## No Testing Infrastructure
- This repository does not include automated tests
- All validation must be done manually using the scenarios above
- No linting or formatting tools are configured
- No CI/CD build validation beyond CodeQL security scanning

## Quick Reference Commands

### Full Development Setup (copy-paste ready)
```bash
# Prerequisites check
dotnet --version  # Should show 10.x.x

# Install workloads and build
dotnet workload install maui
dotnet restore
dotnet build

# Run all applications (3 separate terminals)
# Terminal 1 - API (start first)
cd src/WUPHF.Api/WUPHF.Api && dotnet run

# Terminal 2 - Web App  
cd src/WUPHF.Web/WUPHF.Web && dotnet run

# Terminal 3 - Mobile App (Windows)
cd src/WUPHF.Mobile/WUPHF.Mobile && dotnet run --framework net9.0-windows10.0.19041.0
```

### Verification URLs
- API Health: http://localhost:5043/api/wuphf/health
- Web App: https://localhost:7277
- OpenAPI Docs: http://localhost:5043/openapi/v1.json

Remember: "I thought I was going to be rich. I mean, I still might be." - Ryan Howard