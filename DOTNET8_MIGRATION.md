# .NET 8 Migration Plan

## Current State Analysis

### Target Frameworks
- **WtProgram (F#):** .NET Framework 4.0 (2010)
- **Win32 (C#):** .NET Framework 2.0 (2005)
- **WtDesktop, WtGroup:** .NET Framework 2.0

### Dependencies Analysis

#### NuGet Packages (packages.config format)
```xml
FSharp.Core.4.3.0.0.Microsoft.Signed - v3.0.0.1
FSPowerPack.Core.Community - v3.0.0.0 (DEPRECATED)
FSPowerPack.Linq.Community - v3.0.0.0 (DEPRECATED)
FSPowerPack.Metadata.Community - v3.0.0.0 (DEPRECATED)
FSPowerPack.Parallel.Seq.Community - v3.0.0.0 (DEPRECATED)
System.ValueTuple - v4.3.0
```

#### PowerPack Usage
**Location:** `WtProgram/Shared/Operators.fs:11`
```fsharp
open Microsoft.FSharp.Collections.Tagged
```

**Used in:**
- `Map2` type (line 243): `Tagged.Map<'a, 'b, Comparer<'a>>.Create(...)`
- `Set2` type (line 255): `Tagged.Set<'a, Comparer<'a>>.Create(...)`

**Purpose:** Custom comparers for Map and Set types

### Non-NuGet Dependencies
- Aga.Controls.dll (TreeView)
- Interop.Shell32.dll
- Interop.SHDocVw.dll
- Newtonsoft.Json.dll

## Migration Strategy

### Phase 1: Replace FSharp.PowerPack

#### Option A: Use Modern F# Collections (RECOMMENDED)
Replace `Tagged.Map` and `Tagged.Set` with standard F# collections:

```fsharp
// OLD (PowerPack):
Tagged.Map<'a, 'b, Comparer<'a>>.Create(Comparer<'a>(), items)
Tagged.Set<'a, Comparer<'a>>.Create(Comparer<'a>(), items)

// NEW (Modern F#):
Map<'a, 'b>(items, Comparer<'a>())
Set<'a>(items, Comparer<'a>())
```

**Changes Required:**
- Update `Map2` constructor (Operators.fs:243)
- Update `Set2` constructor (Operators.fs:255)
- Remove `open Microsoft.FSharp.Collections.Tagged` (Operators.fs:11)

**Testing:** All existing Map2/Set2 usage should work identically.

#### Option B: Use FSharpx.Collections
Alternative modern library, but adds unnecessary dependency since we only need Map/Set with custom comparers.

### Phase 2: Migrate to SDK-Style Projects

#### Benefits
- ✅ Simpler, cleaner project files
- ✅ Implicit file globbing (no manual file listing)
- ✅ PackageReference instead of packages.config
- ✅ Multi-targeting support
- ✅ Better tooling support

#### Win32 Project (C#) - SDK-Style

**File:** `Win32/Win32.csproj` (new)

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net8.0-windows</TargetFramework>
    <OutputType>Library</OutputType>
    <RootNamespace>Bemo.Win32</RootNamespace>
    <AssemblyName>Win32</AssemblyName>
    <AllowUnsafeBlocks>true</AllowUnsafeBlocks>
    <UseWindowsForms>true</UseWindowsForms>
    <NoWarn>0649</NoWarn>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="System.Drawing.Common" Version="8.0.0" />
  </ItemGroup>
</Project>
```

**Changes:**
- Target: net2.0 → net8.0-windows
- Format: Legacy → SDK-style
- Files: Explicit → Implicit globbing
- Packages: packages.config → PackageReference

#### WtProgram Project (F#) - SDK-Style

**File:** `WtProgram/WtProgram.fsproj` (new)

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net8.0-windows</TargetFramework>
    <OutputType>WinExe</OutputType>
    <RootNamespace>WindowTabs</RootNamespace>
    <AssemblyName>WindowTabs</AssemblyName>
    <UseWindowsForms>true</UseWindowsForms>
    <ApplicationIcon>Resources\Bemo.ico</ApplicationIcon>
    <StartupObject></StartupObject>
  </PropertyGroup>

  <!-- F# file order is critical -->
  <ItemGroup>
    <Compile Include="Shared/Operators.fs" />
    <Compile Include="Shared/Dynamic.fs" />
    <Compile Include="Shared/Observable.fs" />
    <!-- ... rest of files in order ... -->
  </ItemGroup>

  <ItemGroup>
    <PackageReference Include="FSharp.Core" Version="8.0.0" />
    <PackageReference Include="Newtonsoft.Json" Version="13.0.3" />
    <PackageReference Include="System.ValueTuple" Version="4.5.0" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\Win32\Win32.csproj" />
  </ItemGroup>

  <ItemGroup>
    <Reference Include="Aga.Controls">
      <HintPath>..\Aga.Controls.dll</HintPath>
    </Reference>
    <Reference Include="Interop.Shell32">
      <HintPath>..\Interop.Shell32.dll</HintPath>
    </Reference>
    <Reference Include="Interop.SHDocVw">
      <HintPath>..\Interop.SHDocVw.dll</HintPath>
    </Reference>
  </ItemGroup>

  <ItemGroup>
    <EmbeddedResource Include="Resources\**\*" />
    <EmbeddedResource Include="Properties\**\*.resx" />
  </ItemGroup>
</Project>
```

### Phase 3: Update Dependencies

#### NuGet Packages - .NET 8 Versions

| Package (Old) | Version (Old) | Package (New) | Version (New) |
|---------------|---------------|---------------|---------------|
| FSharp.Core | 4.3.0.0 | FSharp.Core | 8.0.0 |
| FSPowerPack.* | 3.0.0.0 | (Remove - replaced) | N/A |
| System.ValueTuple | 4.3.0 | (Built-in .NET 8) | N/A |
| Newtonsoft.Json | Current | Newtonsoft.Json | 13.0.3 |

#### Optional: Migrate to System.Text.Json

**Consideration:** Newtonsoft.Json works fine on .NET 8, but System.Text.Json is:
- ✅ Built-in (.NET 8)
- ✅ Better performance
- ✅ Lower memory
- ❌ Different API
- ❌ Migration effort

**Recommendation:** Keep Newtonsoft.Json initially, migrate later if needed.

### Phase 4: Fix Breaking Changes

#### F# 3.0 → F# 8.0 Breaking Changes

1. **Implicit conversions:** Some removed, need explicit casts
2. **Computation expressions:** New syntax available
3. **Nullable reference types:** Can enable (optional)
4. **Type inference:** Improved, may reveal hidden issues

#### .NET Framework 4.0 → .NET 8 Breaking Changes

1. **WinForms:** Minor API changes
2. **Threading:** Some APIs updated
3. **Serialization:** BinaryFormatter deprecated (doesn't affect this app)
4. **Crypto:** Some algorithms deprecated (doesn't affect this app)

### Phase 5: Testing Strategy

#### Compilation Testing
```bash
# Clean build
dotnet clean
dotnet build --configuration Debug

# Release build
dotnet build --configuration Release
```

#### Regression Testing
1. Launch application
2. Test window grouping (basic apps)
3. Test Edge PWA grouping (new feature)
4. Test tab drag-and-drop
5. Test settings persistence
6. Test all plugins
7. Test hotkeys
8. Test system tray
9. Test workspace management

## Implementation Steps

### Step 1: Remove PowerPack Dependency (30 min)

**File:** `WtProgram/Shared/Operators.fs`

```fsharp
// Remove line 11:
// open Microsoft.FSharp.Collections.Tagged

// Update line 243 (Map2):
// OLD:
new(?l:List2<_>) = Map2<'a, 'b>(Tagged.Map<'a, 'b, Comparer<'a>>.Create(Comparer<'a>(),(defaultArg l (List2())).list))

// NEW:
new(?l:List2<_>) =
    let items = (defaultArg l (List2())).list
    let comparer = Comparer<'a>()
    Map2<'a, 'b>(Map<'a, 'b>(Seq.map (fun x -> (x, Unchecked.defaultof<'b>)) items, comparer)))

// Update line 255 (Set2):
// OLD:
new(?l:List2<_>) = Set2<'a>(Tagged.Set<'a, Comparer<'a>>.Create(Comparer<'a>(),(defaultArg l (List2())).list))

// NEW:
new(?l:List2<_>) =
    let items = (defaultArg l (List2())).list
    let comparer = Comparer<'a>()
    Set2<'a>(Set<'a>(items, comparer))
```

**Wait...** Let me read the actual Map2/Set2 constructors more carefully...

### Step 2: Create SDK-Style Win32.csproj (15 min)

1. Backup existing: `cp Win32/Win32.csproj Win32/Win32.csproj.old`
2. Create new SDK-style project (see template above)
3. Test build: `dotnet build Win32/Win32.csproj`

### Step 3: Create SDK-Style WtProgram.fsproj (30 min)

1. Backup existing: `cp WtProgram/WtProgram.fsproj WtProgram/WtProgram.fsproj.old`
2. Create new SDK-style project (see template above)
3. Carefully maintain F# file order (critical!)
4. Test build: `dotnet build WtProgram/WtProgram.fsproj`

### Step 4: Update Solution File (10 min)

Update `WindowTabs.sln` if needed for .NET 8 compatibility.

### Step 5: Fix Compilation Errors (1-2 hours)

Iterate through compiler errors:
- Update API calls that changed
- Add explicit type annotations where inference fails
- Fix deprecated API usage

### Step 6: Runtime Testing (2-3 hours)

Full regression test suite on Windows 10/11.

## Rollback Plan

If migration fails:
1. Restore backup project files: `*.csproj.old`, `*.fsproj.old`
2. Restore `Operators.fs` from git
3. Run: `git checkout -- WtProgram/packages.config`
4. Build with original project files

## Timeline Estimate

| Task | Time | Status |
|------|------|--------|
| Remove PowerPack | 30 min | ⏳ Pending |
| SDK-style Win32 | 15 min | ⏳ Pending |
| SDK-style WtProgram | 30 min | ⏳ Pending |
| Fix compilation errors | 1-2 hrs | ⏳ Pending |
| Runtime testing | 2-3 hrs | ⏳ Pending |
| **Total** | **4-6 hrs** | |

## Success Criteria

✅ Project compiles with .NET 8
✅ All existing functionality works
✅ Edge PWA differentiation still works
✅ No performance regression
✅ Smaller deployment size (remove PowerPack DLLs)
✅ Ready for future modernization

## Next Steps After .NET 8 Migration

1. **AOT Compilation:** Consider ahead-of-time compilation for faster startup
2. **Trimming:** Reduce deployment size
3. **Source Generators:** Use for Win32 interop (C# 12 LibraryImport)
4. **Modern F# Features:** async, task, computation expressions
5. **Phase 2:** Architecture modernization (hexagonal architecture)

---

**Status:** 📋 Ready to implement
**Estimated Completion:** Same day (4-6 hours on Windows machine)
**Risk Level:** 🟡 Medium (well-defined, but requires careful testing)
