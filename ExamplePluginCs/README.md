# Cassie Clip Launcher — C# Plugin Development

Guide for writing **C#** plugins for the Cassie Clip Launcher using the `IFnPlugin` SDK. See the `ExamplePluginCs` folder for a full working sample.

---

## Overview

C# is the standard, recommended way to write plugins for the launcher. Plugins are compiled as `.dll` assemblies and dropped into the launcher's plugin directory, where they're discovered and loaded at runtime. Compared to C++/CLI plugins, C# plugins are simpler to build and install — no native interop, no extra runtime shim.

---

## The Plugin SDK Interface

Every plugin must implement `IFnPlugin`:

```csharp
public interface IFnPlugin
{
    string Name { get; }
    string Version { get; }
    string Author { get; }
    string Description { get; }

    Control CreateView();
}
```

| Member | Purpose |
|---|---|
| `Name` | Display name shown in the launcher's plugin list |
| `Version` | Plugin version string |
| `Author` | Your name or handle |
| `Description` | One-line summary of the plugin |
| `CreateView()` | Returns an Avalonia `Control` hosted inside the launcher UI |

Implement the interface on a plain class and return whatever Avalonia control tree you want rendered — anything from a single `TextBlock` up to a full layout with buttons, inputs, etc.

---

## Project Setup

A C# plugin is a standard .NET class library project targeting the same framework as the launcher, with a reference to the launcher SDK and Avalonia:

- Target framework: `net8.0`
- Reference `CassieLauncher.SDK.dll` (the assembly containing `IFnPlugin`) as a non-private reference, so it isn't copied into your output folder
- Add the `Avalonia` NuGet package so your project can build `Control`-derived types

---

## Building

Build with the standard .NET CLI or MSBuild:

```bash
dotnet build -c Release
```

or

```bash
msbuild YourPlugin.csproj /p:Configuration=Release
```

The build output will include your plugin `.dll` in `bin\Release\net8.0\`.

---

## Installing

1. Copy your built `.dll` into:
   ```
   C:\Users\%Username%\AppData\Roaming\com.cassie-launcher\plugins
   ```
2. Click the reload button in the launcher — your plugin should appear in the plugin list.

No extra steps are needed beyond this — unlike C++/CLI plugins, there's no native shim DLL to exclude via `plugin-ignore.json`.

---

## Troubleshooting

| Symptom | Likely Cause | Fix |
|---|---|---|
| Plugin doesn't appear in launcher | Wrong folder, or SDK reference mismatch | Confirm DLL path and that `CassieLauncher.SDK` reference matches the launcher's version |
| Build error referencing `IFnPlugin` | SDK reference missing or `HintPath` wrong | Check the `Reference` entry in your `.csproj` points to a valid `CassieLauncher.SDK.dll` |
| UI doesn't render as expected | Avalonia version mismatch | Match the `Avalonia` package version to the one the launcher uses |