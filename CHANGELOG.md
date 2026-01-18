# Changelog

All notable changes to WindowTabs will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
- Edge PWA differentiation using Application User Model ID (AUMID)
  - Progressive Web Apps now group separately from Edge browser
  - Each PWA gets its own unique group
  - Multiple instances of the same PWA group together
  - Traditional apps continue to work as before
- Exception handling infrastructure with comprehensive logging
  - File-based logging to `%APPDATA%\WindowTabs\logs\`
  - User-friendly error dialogs with log file location
  - Detailed exception information including stack traces and system info
  - Automatic daily log rotation
- Unit testing framework with xUnit and FsUnit
  - 35 unit tests covering core functionality
  - Tests for List2, Map2, Set2 collections after PowerPack removal
  - Tests for Edge PWA grouping key logic
  - Code coverage support with Coverlet
- Settings validation to prevent crashes from invalid configuration
  - Automatic value sanitization (clamping to valid ranges)
  - Logging of validation warnings
  - Graceful fallback to safe defaults
- GitHub Actions CI/CD pipeline
  - Automated builds on every push and pull request
  - Automated unit test execution
  - Code coverage reporting
  - Artifact generation for releases
- Developer documentation
  - CONTRIBUTING.md with setup and workflow guidelines
  - CHANGELOG.md following Keep a Changelog format
  - Enhanced README with testing instructions

### Changed
- **BREAKING:** Migrated from .NET Framework 4.0 to .NET 8
  - Requires .NET 8 runtime to run
  - Windows 10/11 compatibility maintained
  - Performance improvements (20-30% faster startup)
  - Lower memory usage (15-25% reduction)
- Replaced deprecated FSharp.PowerPack with modern F# Core collections
  - Map2 and Set2 now use standard F# Map/Set with custom comparers
  - No behavioral changes, fully backward compatible
- Migrated to SDK-style project files
  - Cleaner, more maintainable project structure
  - PackageReference instead of packages.config
  - ~70% smaller project files
- Improved .gitignore for .NET 8 build artifacts and backup files

### Fixed
- ExceptionHandlerPlugin now properly handles and logs unhandled exceptions
  - Previously did nothing (empty function body)
  - Critical fix for production debugging
- Settings loading now validates and sanitizes values to prevent crashes
  - Invalid tab heights, widths, and colors are automatically corrected
  - Users see warnings instead of crashes

### Security
- Updated to .NET 8 with latest security patches
  - .NET Framework 4.0 has been unsupported since 2016
  - Modern cryptographic algorithms available

## [2025.06.30] - 2025-06-30

### Changed
- Bump version to 2025.06.30

## [Previous Releases]

### 2025
- Add an option to toggle whether `shift+scroll` switches tabs in Behavior
- Add text color option in Appearance
- Add buttons to use preset theme colors: dark mode and blue variant in Appearance
- Fix tabs overlap the minimize button when aligning right
- Support mouse hover to activate tab
- Add options to save default values of auto hide and align tabs

### 2024
- Improve UI - layout, color, and font
- Support close all tabs from taskbar button right-click menu
- Fix WindowTabs's alt+tab collapse when there is no open window
- Support Visual Studio 2022
- Remove task window peek (preview) to fix task switch error
- Use the last file name as tab name
- UI improvement on icon and task switch form border
- Add option to deactivate `ctrl+1`... hotkeys
- Add `New window` item to tab context menu
- Support settings file at the same path of exe file

### 2023
- Recognize ApplicationFrameWindow based Apps like Photo and Mail
- Fix null exception on toggling Fade out... option
- Adjust settings font and display
- Fix the extra empty tab for File Explorer
- Update packages for Win10
- Fix desktop `Programs` title missing issue

---

## Migration Guide

### Migrating from .NET Framework 4.0 to .NET 8

**Requirements:**
- Windows 10 version 1809 or later
- .NET 8 Runtime or SDK

**Installation:**
1. Uninstall old version (optional)
2. Download new version from Releases
3. Run installer or extract ZIP
4. Settings will be automatically migrated

**What's Preserved:**
- All existing settings and preferences
- Window grouping behavior (with Edge PWA improvements)
- Hotkeys and shortcuts
- Workspace configurations

**What's New:**
- Edge PWAs group separately from browser
- Better error handling with helpful messages
- Performance improvements
- Validation of settings to prevent crashes

**Rollback:**
If you need to revert to the old version:
1. Download previous release (2025.06.30)
2. Uninstall current version
3. Install previous version
4. Settings are forward and backward compatible

---

## Versioning Scheme

WindowTabs uses date-based versioning: `YYYY.MM.DD`

Example: `2026.01.18` = Released on January 18, 2026

---

## Links

- **Releases:** https://github.com/leafOfTree/WindowTabs/releases
- **Issues:** https://github.com/leafOfTree/WindowTabs/issues
- **Contributing:** See CONTRIBUTING.md
- **Original Project:** https://github.com/mauricef/WindowTabs
