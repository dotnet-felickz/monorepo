#!/bin/bash

echo "🐕 WUPHF! Quick Start Script"
echo "==============================================="
echo "Welcome to Ryan Howard's Ultimate Social Networking Experience!"
echo ""

# Check if .NET is installed
if ! command -v dotnet &> /dev/null; then
    echo "❌ .NET is not installed. Please install .NET 10 SDK first."
    echo "   Visit: https://dotnet.microsoft.com/download"
    exit 1
fi

echo "✅ .NET is installed"

# Check .NET version
DOTNET_VERSION=$(dotnet --version)
echo "   Version: $DOTNET_VERSION"

# Install MAUI workload
echo ""
echo "📱 Installing MAUI workload for mobile development..."
dotnet workload install maui

# Optional: Install Android SDK components for MAUI Android if environment is configured
MOBILE_PROJ="src/WUPHF.Mobile/WUPHF.Mobile/WUPHF.Mobile.csproj"
if [ -f "$MOBILE_PROJ" ]; then
    ANDROID_TFM=$(grep -Eo 'net[0-9]+\.[0-9]+-android' "$MOBILE_PROJ" | head -n1)
fi

if [ -n "$ANDROID_TFM" ] && [ -n "${ANDROID_SDK_ROOT:-}" ] && [ -n "${JAVA_HOME:-}" ]; then
    echo ""
    echo "🔧 Installing Android dependencies for $ANDROID_TFM (using ANDROID_SDK_ROOT and JAVA_HOME)..."
    dotnet build "$MOBILE_PROJ" \
        -t:InstallAndroidDependencies \
        -f "$ANDROID_TFM" \
        -p:AndroidSdkDirectory="$ANDROID_SDK_ROOT" \
        -p:JavaSdkDirectory="$JAVA_HOME" \
        -p:AcceptAndroidSdkLicenses=True -v minimal || \
        echo "⚠️ Android dependency install step had issues; continuing"
else
    echo ""
    echo "ℹ️ Skipping Android dependency installation (set ANDROID_SDK_ROOT and JAVA_HOME to enable)"
fi

# Restore packages
echo ""
echo "📦 Restoring NuGet packages..."
dotnet restore

# Build the solution
echo ""
echo "🔨 Building WUPHF solution..."
dotnet build

if [ $? -eq 0 ]; then
    # Build Mobile (Windows target by default if on Windows via WSL/WSLg not applicable here; keep Android optional)
    if [ -f "$MOBILE_PROJ" ]; then
        if [ -n "$ANDROID_TFM" ] && [ -n "${ANDROID_SDK_ROOT:-}" ] && [ -n "${JAVA_HOME:-}" ]; then
            echo ""
            echo "📦 Building MAUI Mobile for Android ($ANDROID_TFM) ..."
            dotnet build "$MOBILE_PROJ" -f "$ANDROID_TFM" -p:AndroidSdkDirectory="$ANDROID_SDK_ROOT" -p:JavaSdkDirectory="$JAVA_HOME" -v minimal || true
        else
            echo ""
            echo "ℹ️ Android build skipped (set ANDROID_SDK_ROOT and JAVA_HOME to enable)"
        fi
    fi

    echo ""
    echo "🎉 WUPHF is ready to go!"
    echo ""
    echo "Ryan Howard says: 'I thought I was going to be rich. I mean, I still might be.'"
    echo ""
    echo "Next steps:"
    echo "1. Run the API: cd src/WUPHF.Api/WUPHF.Api && dotnet run"
    echo "2. Run the Web App: cd src/WUPHF.Web/WUPHF.Web && dotnet run"
    echo "3. Run the Mobile App: cd src/WUPHF.Mobile/WUPHF.Mobile && dotnet run"
    echo ""
    echo "🚀 Start WUPHFing!"
else
    echo ""
    echo "❌ Build failed. Ryan is not pleased."
    echo "Check the error messages above and try again."
    exit 1
fi
