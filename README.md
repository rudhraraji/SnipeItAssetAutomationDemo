# Snipe-IT Asset Automation Demo

Automated UI testing project for the [Snipe-IT Demo](https://demo.snipeitapp.com), built using C#, Playwright, and NUnit.

## Overview

This project automates the asset creation and verification process in Snipe-IT.

The tests cover:

1. Logging in as an admin user.
2. Creating a Macbook Pro 13" asset with the Ready to Deploy status.
3. Checking out the asset to a randomly selected user.
4. Searching for the asset using its asset tag.
5. Verifying the asset details, including model, status, and assigned user.
6. Checking the History tab to verify asset creation and checkout activities.

## Technologies Used

- C#
- .NET 10
- Microsoft Playwright
- NUnit
- HTML test reports
- Git and GitHub

## Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/rudhraraji/SnipeItAssetAutomationDemo.git
cd SnipeItAssetAutomationDemo
```

### 2. Build the project

```bash
dotnet build
```

### 3. Install the Playwright browser

Windows (PowerShell):

```powershell
powershell -ExecutionPolicy Bypass -File bin/Debug/net10.0/playwright.ps1 install chromium
```

macOS / Linux:

```bash
pwsh bin/Debug/net10.0/playwright.ps1 install chromium
```

On Linux, install the required system dependencies if needed:

```bash
pwsh bin/Debug/net10.0/playwright.ps1 install --with-deps chromium
```

## Running the Tests

Run these commands from the project folder.

| Action | Command |
|---|---|
| Run all tests | `dotnet test` |
| Run without displaying the browser | `dotnet test -p:Headless=true` |
| Run with a visible browser | `dotnet test -p:Headless=false` |
| Run the asset workflow test | `dotnet test --filter "Name=AssetWorkflow"` |
| Run login tests only | `dotnet test --filter "FullyQualifiedName~LoginTests"` |

By default, the browser is visible during test execution. You can change this in the configuration file.

## Test Report

An HTML test report is generated after a test run.

**Report location:**

```text
TestResults/TestReport.html
```

The report includes:
- Test results
- Execution duration
- Error messages for failed tests

Open the report in your browser.

Windows PowerShell:

```powershell
Invoke-Item TestResults\TestReport.html
```

Run `dotnet test` first if the report does not exist.

## Configuration

Test settings are stored in `SnipeIt.config.json`.

Example:

```json
{
  "baseUrl": "https://demo.snipeitapp.com",
  "headless": false,
  "accounts": {
    "admin": {
      "username": "admin",
      "password": "password",
      "displayName": "Admin User"
    },
    "demoUser": {
      "username": "testuser",
      "password": "password",
      "displayName": "Test User"
    }
  },
  "newAsset": {
    "model": "Macbook Pro 13",
    "status": "Ready to Deploy"
  }
}
```

You can update the configuration to change:

- Snipe-IT site URL
- Login credentials
- Browser mode
- Asset model
- Asset status

Changes take effect on the next test run.

## Environment Variables

You can override configuration settings using environment variables.

| Variable | Purpose |
|---|---|
| `SNIPEIT_BASE_URL` | Snipe-IT site URL |
| `SNIPEIT_ADMIN_USERNAME` | Admin username |
| `SNIPEIT_ADMIN_PASSWORD` | Admin password |
| `SNIPEIT_ADMIN_DISPLAY_NAME` | Admin display name |
| `SNIPEIT_USER_USERNAME` | Test user username |
| `SNIPEIT_USER_PASSWORD` | Test user password |
| `SNIPEIT_USER_DISPLAY_NAME` | Test user display name |
| `SNIPEIT_ASSET_MODEL` | Asset model |
| `SNIPEIT_ASSET_STATUS` | Asset status |

Example for Windows PowerShell:

```powershell
$env:SNIPEIT_BASE_URL = "https://snipeit.example.com"
dotnet test
```

## Project Goal

This project demonstrates practical skills in:

- UI test automation
- End-to-end testing
- Test data validation
- Reusable automated tests
- Test reporting
- C# and Playwright automation

Repository: [SnipeItAssetAutomationDemo](https://github.com/rudhraraji/SnipeItAssetAutomationDemo)
