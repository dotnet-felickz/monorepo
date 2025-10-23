# WUPHF Refactoring - Testing Guide

## Overview
This document outlines the testing that should be performed after the refactoring changes to ensure all functionality remains intact.

## Prerequisites
- .NET 10 SDK installed (required for building and running)
- API project running on localhost:7000 (for Web and Mobile testing)

## What Was Changed
1. Moved validation logic from API Controller to shared `WuphfValidationService`
2. Moved UI helper functions to shared `WuphfUIHelper`
3. Created shared `WuphfMessageViewModel` for consistent data display
4. Created shared `WuphfApiClient` for HTTP communication (available but not yet used by Web/Mobile)
5. Updated all projects to use shared constants instead of hardcoded values

## Test Scenarios

### 1. API Testing (Critical)

#### Test 1.1: Health Check
- Endpoint: `GET http://localhost:7000/api/wuphf/health`
- Expected: 200 OK with health status JSON
- Purpose: Verify API is running

#### Test 1.2: Validate Empty Message
- Endpoint: `POST http://localhost:7000/api/wuphf/send`
- Body:
```json
{
  "fromUser": "Test User",
  "toUser": "Ryan Howard",
  "message": "",
  "channels": [0, 1],
  "printWuphf": true
}
```
- Expected: 400 Bad Request with error "Message cannot be empty! Ryan says: 'You can't WUPHF nothing!'"
- Purpose: Verify shared validation service works for empty messages

#### Test 1.3: Validate Message Too Long
- Endpoint: `POST http://localhost:7000/api/wuphf/send`
- Body: Same as above but with message > 280 characters
- Expected: 400 Bad Request with error about message length
- Purpose: Verify shared validation service works for length validation

#### Test 1.4: Validate No Channels
- Endpoint: `POST http://localhost:7000/api/wuphf/send`
- Body:
```json
{
  "fromUser": "Test User",
  "toUser": "Ryan Howard",
  "message": "Test message",
  "channels": [],
  "printWuphf": false
}
```
- Expected: 400 Bad Request with error "No channels selected! The whole point of WUPHF is to send everywhere!"
- Purpose: Verify shared validation service works for channel validation

#### Test 1.5: Successful Send
- Endpoint: `POST http://localhost:7000/api/wuphf/send`
- Body:
```json
{
  "fromUser": "Test User",
  "toUser": "Ryan Howard",
  "message": "This is a test WUPHF message!",
  "channels": [0, 1, 2, 3],
  "printWuphf": true
}
```
- Expected: 200 OK with SendWuphfResponse including messageId and channel counts
- Purpose: Verify end-to-end send functionality still works

#### Test 1.6: Get History
- Endpoint: `GET http://localhost:7000/api/wuphf/history`
- Expected: 200 OK with array of WuphfMessage objects
- Purpose: Verify history retrieval works

### 2. Web Application Testing

#### Test 2.1: Send WUPHF Page UI
- Navigate to: `https://localhost:7277/send`
- Verify:
  - All channel checkboxes display with correct icons (📘, 🐦, 💬, etc.)
  - Character counter displays "0 / 280 characters"
  - Form elements render correctly
- Purpose: Verify UI helpers work correctly for channel icons

#### Test 2.2: Web Validation - Empty Message
- On Send WUPHF page
- Leave message empty, fill in From and To, select channels
- Click "Send WUPHF!"
- Expected: Validation error displayed
- Purpose: Verify client-side/server-side validation works

#### Test 2.3: Web Validation - Message Too Long
- On Send WUPHF page
- Enter message > 280 characters
- Click "Send WUPHF!"
- Expected: Validation error about length
- Purpose: Verify length validation works

#### Test 2.4: Web Successful Send
- On Send WUPHF page
- Fill all fields correctly, select multiple channels
- Click "Send WUPHF!"
- Expected: 
  - Success message displayed
  - Delivery results shown with channel success/failure
  - Ryan's Reaction displayed
- Purpose: Verify complete send flow works

#### Test 2.5: History Page Display
- Navigate to: `https://localhost:7277/history`
- Verify:
  - Messages display with correct channel icons
  - Status badges show correct colors (green for success, red for failure, etc.)
  - Status icons display correctly (✅, ❌, ⚠️, etc.)
  - Message details modal works when clicking "Details"
- Purpose: Verify all UI helpers work correctly in history display

### 3. Mobile Application Testing

#### Test 3.1: Send WUPHF Page
- Launch mobile app
- Navigate to "Send WUPHF" page
- Verify:
  - Character counter shows "0 / 280 characters" (using constant)
  - Counter turns red when > 280, orange when > 240
  - All channel checkboxes present
- Purpose: Verify constants are used correctly

#### Test 3.2: Mobile Validation
- Try sending with empty message
- Try sending with message > 280 characters
- Try sending with no channels selected
- Expected: Appropriate error dialogs
- Purpose: Verify validation works on mobile

#### Test 3.3: Mobile History Page
- Navigate to History page
- Verify:
  - Messages display correctly
  - Status colors are correct (using shared view model)
  - No compilation errors or crashes
- Purpose: Verify shared view model works with MAUI Color conversion

### 4. Build and Compilation Testing

#### Test 4.1: Build Shared Project
```bash
cd src/WUPHF.Shared
dotnet build
```
- Expected: Successful build with no errors
- Purpose: Verify shared library compiles correctly

#### Test 4.2: Build API Project
```bash
cd src/WUPHF.Api
dotnet build
```
- Expected: Successful build with no errors
- Purpose: Verify API project builds with new dependencies

#### Test 4.3: Build Web Project
```bash
cd src/WUPHF.Web
dotnet build
```
- Expected: Successful build with no errors
- Purpose: Verify Web project builds with new dependencies

#### Test 4.4: Build Mobile Project
```bash
cd src/WUPHF.Mobile
dotnet build
```
- Expected: Successful build with no errors
- Purpose: Verify Mobile project builds with changes

#### Test 4.5: Build Entire Solution
```bash
dotnet build
```
- Expected: All projects build successfully
- Purpose: Verify no breaking changes across solution

## Regression Testing Checklist

### API
- [ ] Health check endpoint works
- [ ] Validation errors are returned correctly
- [ ] Successful send returns proper response
- [ ] History endpoint returns data
- [ ] Quotes endpoint still works
- [ ] Channel simulation logic unchanged

### Web
- [ ] Send page displays correctly
- [ ] All channel icons display correctly
- [ ] Validation messages display correctly
- [ ] Send functionality works end-to-end
- [ ] History page displays correctly
- [ ] Status icons and colors are correct
- [ ] Message details modal works
- [ ] Filtering works

### Mobile
- [ ] App launches without crashes
- [ ] Send page displays correctly
- [ ] Character counter works correctly
- [ ] Validation dialogs display correctly
- [ ] History page displays correctly
- [ ] Status colors are correct

## Success Criteria
- All API endpoints return expected responses
- Web application displays all UI elements correctly
- Mobile application compiles and runs without errors
- No duplicate code remains in application projects
- All validation logic centralized in shared library
- All UI helper logic centralized in shared library
- No breaking changes to existing functionality

## Notes
- The WuphfApiClient is available in Shared but not yet consumed by Web/Mobile (future enhancement)
- All hardcoded "280" values have been replaced with WuphfConstants.Limits.MaxMessageLength
- Validation error messages remain identical to preserve user experience
- UI element styling and behavior unchanged
