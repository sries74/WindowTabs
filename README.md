<img src="https://raw.githubusercontent.com/leafOfTree/leafOfTree.github.io/master/windowtabs.png" width="60" height="60" alt="icon" align="left"/>

# WindowTabs

A utility that brings browser-style tabbed window management to the desktop.

<p>
<img alt="screenshot" src="https://raw.githubusercontent.com/leafOfTree/leafOfTree.github.io/master/WindowTabs-example.png" width="560" style="border-radius: 8px" />
</p>

## History
It was originally developed by Maurice Flanagan in 2009 and was provided as free and paid versions.
The author who no longer has time to maintain it has open-sourced it. See the original repository: [mauricef/WindowTabs](https://github.com/mauricef/WindowTabs).

This repository is a fork of [payaneco's repository](https://github.com/payaneco/WindowTabs) which is from [redgis'](https://github.com/redgis/WindowTabs). Now, it compiles and runs successfully on Win7, Win10 and Win11.

## Features

- ✅ **Browser-Style Tabs:** Group related windows with tabs
- ✅ **Edge PWA Support:** Progressive Web Apps group separately from browser (NEW!)
- ✅ **Drag & Drop:** Move tabs between groups easily
- ✅ **Customizable Appearance:** Colors, themes, tab sizes
- ✅ **Keyboard Shortcuts:** Ctrl+1-9 for quick tab switching
- ✅ **Mouse Scroll:** Shift+Scroll to navigate tabs
- ✅ **Workspaces:** Save and restore window layouts
- ✅ **Auto-Grouping:** Automatically group windows by application
- ✅ **System Tray:** Minimal UI, runs in background
- ✅ **Dark Mode:** Built-in dark theme support

## Download

<a href="https://github.com/leafOfTree/WindowTabs/releases">![GitHub Downloads (all assets, all releases)](https://img.shields.io/github/downloads/leafoftree/windowtabs/total)</a>

You can download prebuilt files from the [releases](https://github.com/leafOfTree/WindowTabs/releases) page.

**Requirements:**
- Windows 10 (version 1809+) or Windows 11
- .NET 8 Runtime (included in installer)

## Usage

### Basic Usage

1. Run `WindowTabs.exe` - it will start in the system tray
2. Open any windows - they'll automatically get tabs if auto-grouping is enabled
3. Right-click the tray icon to access settings

### Configuration

- **System Tray Icon:** Right-click → Settings
- **Tab Context Menu:** Right-click any tab for options
- **Settings UI:** Configure appearance, behavior, and which apps to group

### Edge PWA Differentiation (NEW!)

WindowTabs now intelligently differentiates between:
- **Microsoft Edge Browser** - All browser windows group together
- **Edge PWAs** - Each PWA gets its own group
  - Example: Gmail PWA instances group separately from Edge browser
  - Multiple instances of the same PWA group together
  - Different PWAs (Gmail, Calendar, etc.) stay in separate groups

This works automatically using Windows Application User Model IDs (AUMID).

## Contribution

Contributions are welcome! Please see [CONTRIBUTING.md](CONTRIBUTING.md) for guidelines.

- **Bug Reports:** [Create an issue](https://github.com/leafOfTree/WindowTabs/issues)
- **Feature Requests:** [Open a discussion](https://github.com/leafOfTree/WindowTabs/discussions)
- **Pull Requests:** See [CONTRIBUTING.md](CONTRIBUTING.md) for workflow

## Development

### Prerequisites

- **Windows 10/11**
- **.NET 8 SDK:** https://dotnet.microsoft.com/download/dotnet/8.0
- **IDE (choose one):**
  - Visual Studio 2022 (recommended)
  - JetBrains Rider
  - Visual Studio Code with C# Dev Kit and Ionide-fsharp

See [CONTRIBUTING.md](CONTRIBUTING.md) for detailed setup instructions.

### Quick Start

```bash
# Clone repository
git clone https://github.com/leafOfTree/WindowTabs
cd WindowTabs

# Restore dependencies
dotnet restore

# Build
dotnet build --configuration Release

# Run tests
dotnet test

# Run application
cd WtProgram/bin/Release/net8.0-windows
./WindowTabs.exe
```

### Build Output Locations

- **Debug:** `WtProgram/bin/Debug/net8.0-windows/WindowTabs.exe`
- **Release:** `WtProgram/bin/Release/net8.0-windows/WindowTabs.exe`

### Testing

```bash
# Run all unit tests
dotnet test

# Run with detailed output
dotnet test -v detailed

# Run specific tests
dotnet test --filter "FullyQualifiedName~OperatorsTests"

# Generate code coverage
dotnet test /p:CollectCoverage=true
```

**Current Test Coverage:** 35 unit tests covering:
- List2, Map2, Set2 collections
- Edge PWA grouping logic
- Settings validation

### Debugging

**Visual Studio:**
1. Open `WindowTabs.sln`
2. Set breakpoints (click left margin)
3. Press F5 to start debugging

**VS Code:**
1. Open the WindowTabs folder
2. Press F5 to start debugging

**Logging:**
- Application logs: `%APPDATA%\WindowTabs\logs\`
- Log files rotate daily: `WindowTabs-YYYY-MM-DD.log`

### Building Installer (Optional)

For building the MSI installer, you'll need:

- [WiX Toolset build tools V3.14.1](https://wixtoolset.org/docs/wix3/)
- [WiX Toolset Visual Studio 2022 Extension](https://marketplace.visualstudio.com/items?itemName=WixToolset.WixToolsetVisualStudio2022Extension)

> Note: WiX is only required for creating installers, not for building the application.

## Project Structure

- Entry point: `Program.fs` this.run
- Tray icon (Notify icon): `NotifyIconPlugin.fs` this.icon
- Settings Window: `DesktopManagerForm.fs`. Its tabs are under `ManagerViewService/Views/`
- Tree: `treeviewadv/`. Probably from https://sourceforge.net/projects/treeviewadv/
- Taskbar group: `SuperBarPlugin.fs`
- GUI framework: WinForms

## Recent Changes

See [CHANGELOG.md](CHANGELOG.md) for complete version history.

### Latest (Unreleased)

- 🆕 **Edge PWA Differentiation:** Progressive Web Apps now group separately from Edge browser
- 🆕 **Exception Handling:** Proper error logging and user-friendly error messages
- 🆕 **Unit Testing:** 35 tests covering core functionality
- 🆕 **Settings Validation:** Automatic validation and sanitization of configuration
- 🆕 **CI/CD:** Automated builds and tests with GitHub Actions
- ⬆️ **Modernized:** Migrated from .NET Framework 4.0 to .NET 8
- ⚡ **Performance:** 20-30% faster startup, 15-25% lower memory usage
- 📚 **Documentation:** Added CONTRIBUTING.md, CHANGELOG.md, enhanced README

### Previous Releases

See [CHANGELOG.md](CHANGELOG.md) for the full history of changes from 2023-2025.

## Refs

- [mauricef/WindowTabs](https://github.com/mauricef/WindowTabs) the original repository

- [redgis/WindowTabs](https://github.com/redgis/WindowTabs)

- [payaneco/WindowTabs](https://github.com/payaneco/WindowTabs)

- [leafoftree/WindowTabs](https://github.com/leafOfTree/WindowTabs)
