#!/usr/bin/env pwsh
$ErrorActionPreference = "Stop"

if ($env:JAVA_HOME) {
    Write-Host "JAVA_HOME is set to: $env:JAVA_HOME"
} else {
    Write-Host "JAVA_HOME is NOT set. Required for Android target net9.0-android"
}

if ($env:ANDROID_HOME) {
    Write-Host "ANDROID_HOME is set to: $env:ANDROID_HOME"
} else {
    Write-Host "ANDROID_HOME is NOT set. Required for Android target net9.0-android"
}


Write-Host "📦 dotnet restore" -ForegroundColor Cyan
dotnet restore

Write-Host "� dotnet build" -ForegroundColor Cyan
dotnet build
