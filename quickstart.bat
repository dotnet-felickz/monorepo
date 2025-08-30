@echo off
echo 🐕 WUPHF! Quick Start Script
echo ===============================================
echo Welcome to Ryan Howard's Ultimate Social Networking Experience!
echo.

REM Check if .NET is installed
where dotnet >nul 2>nul
if %ERRORLEVEL% NEQ 0 (
    echo ❌ .NET is not installed. Please install .NET 10 SDK first.
    echo    Visit: https://dotnet.microsoft.com/download
    pause
    exit /b 1
)

echo ✅ .NET is installed

REM Check .NET version
for /f "tokens=*" %%a in ('dotnet --version') do set DOTNET_VERSION=%%a
echo    Version: %DOTNET_VERSION%

REM Install MAUI workload
echo.
echo 📱 Installing MAUI workload for mobile development...
dotnet workload install maui

REM Restore packages
echo.
echo 📦 Restoring NuGet packages...
dotnet restore

REM Build the solution
echo.
echo 🔨 Building WUPHF solution...
dotnet build

if %ERRORLEVEL% EQU 0 (
    echo.
    echo 🎉 WUPHF is ready to go!
    echo.
    echo Ryan Howard says: "I thought I was going to be rich. I mean, I still might be."
    echo.
    echo Next steps:
    echo 1. Run the API: cd src\WUPHF.Api\WUPHF.Api ^&^& dotnet run
    echo 2. Run the Web App: cd src\WUPHF.Web\WUPHF.Web ^&^& dotnet run
    echo 3. Run the Mobile App: cd src\WUPHF.Mobile\WUPHF.Mobile ^&^& dotnet run
    echo.
    echo 🚀 Start WUPHFing!
) else (
    echo.
    echo ❌ Build failed. Ryan is not pleased.
    echo Check the error messages above and try again.
    pause
    exit /b 1
)

pause
