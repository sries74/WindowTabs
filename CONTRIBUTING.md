# Contributing to WindowTabs

Thank you for your interest in contributing to WindowTabs! This guide will help you get started.

## Table of Contents

- [Code of Conduct](#code-of-conduct)
- [Getting Started](#getting-started)
- [Development Setup](#development-setup)
- [Project Structure](#project-structure)
- [Development Workflow](#development-workflow)
- [Testing](#testing)
- [Coding Standards](#coding-standards)
- [Submitting Changes](#submitting-changes)
- [Plugin Development](#plugin-development)

## Code of Conduct

Be respectful, considerate, and professional in all interactions. We aim to maintain a welcoming community for everyone.

## Getting Started

### Prerequisites

- **Operating System:** Windows 10/11
- **.NET 8 SDK:** https://dotnet.microsoft.com/download/dotnet/8.0
- **IDE (choose one):**
  - Visual Studio 2022 Community (recommended)
  - JetBrains Rider
  - Visual Studio Code with C# Dev Kit and Ionide-fsharp

### Optional Tools

- **WiX Toolset 3.14.1:** For building installers (only needed for release builds)
- **Git:** For version control

## Development Setup

### 1. Fork and Clone

```bash
# Fork the repository on GitHub, then:
git clone https://github.com/YOUR_USERNAME/WindowTabs.git
cd WindowTabs
```

### 2. Install Dependencies

```bash
dotnet restore
```

### 3. Build

```bash
# Debug build
dotnet build --configuration Debug

# Release build
dotnet build --configuration Release
```

### 4. Run

```bash
cd WtProgram/bin/Debug/net8.0-windows
./WindowTabs.exe
```

## Project Structure

```
WindowTabs/
├── Win32/                      # C# Win32 API interop layer
│   ├── Win32Helper.cs         # Helper methods for Win32 APIs
│   ├── ShellApi.cs            # Shell API P/Invokes
│   └── WinUser.cs             # User32 API definitions
│
├── WtProgram/                 # F# main application
│   ├── Shared/                # Shared utilities and types
│   │   ├── Operators.fs       # List2, Map2, Set2 collections
│   │   ├── Win32.fs           # F# Win32 wrapper (OS, Window, Pid)
│   │   ├── Logger.fs          # Logging infrastructure
│   │   └── SettingsValidator.fs # Settings validation
│   │
│   ├── DesktopPlugins/        # Desktop-level plugins
│   │   ├── ExceptionHandlerPlugin.fs
│   │   ├── NotifyIconPlugin.fs
│   │   └── InputManagerPlugin.fs
│   │
│   ├── GroupPlugins/          # Window group plugins
│   │   ├── SuperBarPlugin.fs  # Taskbar integration
│   │   ├── MouseScrollPlugin.fs
│   │   └── NumericTabHotKeyPlugin.fs
│   │
│   ├── ManagerViewService/    # Settings UI
│   │   └── Views/             # Settings view tabs
│   │
│   ├── Settings.fs            # Settings management
│   ├── WindowGroup.fs         # Window grouping logic
│   ├── TabStrip.fs            # Tab strip rendering
│   └── Program.fs             # Application entry point
│
└── WindowTabs.Tests/          # Unit tests
    ├── OperatorsTests.fs      # Tests for List2/Map2/Set2
    └── GroupingKeyTests.fs    # Tests for Edge PWA differentiation
```

## Development Workflow

### 1. Create a Feature Branch

```bash
git checkout -b feature/your-feature-name
# or
git checkout -b bugfix/issue-number-description
```

### 2. Make Your Changes

- Follow the [coding standards](#coding-standards) below
- Write tests for new functionality
- Update documentation as needed

### 3. Test Locally

```bash
# Run unit tests
dotnet test

# Run the application
dotnet run --project WtProgram/WtProgram.fsproj
```

### 4. Commit Changes

```bash
git add .
git commit -m "Brief description of changes

More detailed explanation of what changed and why.

Fixes #123"
```

**Commit Message Guidelines:**
- Use present tense: "Add feature" not "Added feature"
- Use imperative mood: "Fix bug" not "Fixes bug"
- First line: brief summary (50 chars max)
- Blank line, then detailed description
- Reference issues: "Fixes #123" or "Relates to #456"

### 5. Push and Create Pull Request

```bash
git push origin feature/your-feature-name
```

Then create a pull request on GitHub.

## Testing

### Running Tests

```bash
# Run all tests
dotnet test

# Run specific test file
dotnet test --filter "FullyQualifiedName~OperatorsTests"

# Run with coverage
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

### Writing Tests

Place tests in `WindowTabs.Tests/` directory. Example:

```fsharp
module WindowTabs.Tests.MyFeatureTests

open Xunit
open FsUnit.xUnit

[<Fact>]
let ``My feature should work correctly`` () =
    let result = myFunction input
    result |> should equal expectedValue
```

### Test Categories

- **Unit Tests:** Test individual functions/modules in isolation
- **Integration Tests:** Test interactions between components
- **Regression Tests:** Prevent previously fixed bugs from reoccurring

## Coding Standards

### F# Style Guide

**Naming Conventions:**
```fsharp
// PascalCase for types and modules
type MyType = ...
module MyModule = ...

// camelCase for functions and values
let myFunction x = ...
let myValue = 42

// Use 'this' consistently (not 'x' or other names)
type MyClass() as this =
    member this.myMethod() = ...
```

**Formatting:**
```fsharp
// 4-space indentation
let myFunction parameter =
    let intermediate = doSomething parameter
    doSomethingElse intermediate

// Line length: aim for < 120 characters
// Break long lines logically
let longFunctionCall =
    functionWithManyParameters
        parameter1
        parameter2
        parameter3
```

**Pattern Matching:**
```fsharp
// Prefer pattern matching over if/else
match value with
| Some x -> doSomething x
| None -> doDefault()

// Use active patterns where appropriate
let (|ValidValue|InvalidValue|) x =
    if validate x then ValidValue x
    else InvalidValue
```

### C# Style Guide

**Naming Conventions:**
```csharp
// PascalCase for classes, methods, properties
public class MyClass
{
    public void MyMethod() { }
    public int MyProperty { get; set; }
}

// camelCase for local variables and parameters
int myVariable = 42;
void MyMethod(int myParameter) { }

// _camelCase for private fields (optional)
private int _myField;
```

**Win32 API Conventions:**
```csharp
// Use [DllImport] with full configuration
[DllImport("user32.dll", SetLastError = true)]
public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

// Use proper error handling
IntPtr hWnd = ...;
if (hWnd == IntPtr.Zero)
{
    int error = Marshal.GetLastWin32Error();
    // Handle error
}
```

### General Guidelines

1. **Error Handling:**
   - Use Result types for recoverable errors
   - Log errors with Logger module
   - Show user-friendly messages (no stack traces to users)

2. **Performance:**
   - Cache expensive operations
   - Use Cell.cacheProp for lazy evaluation
   - Profile before optimizing

3. **Documentation:**
   - Add XML doc comments for public APIs
   - Explain "why" not just "what" in comments
   - Update docs when changing behavior

4. **Dependencies:**
   - Minimize external dependencies
   - Use built-in .NET types where possible
   - Document any new dependencies added

## Submitting Changes

### Pull Request Guidelines

**Before Submitting:**
- ✅ Code compiles without warnings
- ✅ All tests pass
- ✅ New tests added for new functionality
- ✅ Code follows style guidelines
- ✅ Documentation updated
- ✅ Commit messages are clear

**PR Title Format:**
```
[Type] Brief description

Types: Feature, Bugfix, Refactor, Docs, Test, Chore
```

**PR Description Template:**
```markdown
## Description
Brief description of changes

## Motivation
Why is this change needed?

## Changes Made
- Change 1
- Change 2
- Change 3

## Testing
How was this tested?

## Screenshots
(if applicable)

## Related Issues
Fixes #123
Relates to #456

## Checklist
- [ ] Code compiles
- [ ] Tests pass
- [ ] New tests added
- [ ] Documentation updated
```

### Code Review Process

1. **Automated Checks:** CI/CD runs automatically
2. **Code Review:** Maintainer reviews code
3. **Feedback:** Address review comments
4. **Approval:** PR approved by maintainer
5. **Merge:** Maintainer merges PR

## Plugin Development

WindowTabs has a plugin architecture for extending functionality.

### Plugin Types

**IPlugin (Base Interface):**
```fsharp
type IPlugin =
    abstract member init: unit -> unit
```

**IDesktopPlugin (Desktop-Level):**
- Monitors all window groups
- Examples: NotifyIconPlugin, InputManagerPlugin

**IGroupPlugin (Group-Level):**
- Operates on individual window groups
- Examples: SuperBarPlugin, MouseScrollPlugin

### Creating a Plugin

```fsharp
// 1. Create plugin file in appropriate directory
namespace Bemo

type MyPlugin() as this =

    // Initialize plugin
    member this.init() =
        Logger.info "MyPlugin initialized"
        // Set up event handlers, etc.

    // Implement plugin interface
    interface IDesktopPlugin with
        member x.init() = this.init()

        member x.onGroupCreated(group) =
            Logger.info "Group created"
            // Handle group creation

// 2. Register plugin in Program.fs
let myPlugin = MyPlugin() :> IPlugin
plugins.Add(myPlugin)
```

### Plugin Best Practices

- Use Logger for diagnostic output
- Handle errors gracefully (don't crash the app)
- Clean up resources in Dispose()
- Document plugin configuration options
- Add tests for plugin functionality

## Getting Help

- **Issues:** https://github.com/leafOfTree/WindowTabs/issues
- **Discussions:** Use GitHub Discussions for questions
- **Pull Requests:** Ask questions in PR comments

## License

By contributing, you agree that your contributions will be licensed under the same license as the project.

---

Thank you for contributing to WindowTabs! 🎉
