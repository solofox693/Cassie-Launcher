# Cassie Clip Launcher — C++ Plugin Development

Guide for writing native **C++/CLI** plugins for the Cassie Clip Launcher using the `IFnPlugin` SDK. See the `ExamplePLuginCpp` example folder for a full working sample.

---

## Overview

Plugins are compiled as separate `.dll` assemblies and dropped into the launcher's plugin directory, where they're discovered and loaded at runtime. Most plugins are plain C#, but you can also write them in **C++/CLI** — this lets you wrap native C++ code behind a managed shim that the launcher can load like any other plugin.

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

For C++/CLI plugins, keep native logic in a plain C++ class and wrap it in a `public ref class` that implements `IFnPlugin`. Any native resources should be cleaned up via a destructor (`~ClassName()`, maps to `Dispose()`) and a finalizer (`!ClassName()`) as a backstop.

---

## Building

Build with MSBuild, targeting Release/x64:

```bash
msbuild YourPlugin.vcxproj -t:restore /p:Configuration=Release /p:Platform=x64
msbuild YourPlugin.vcxproj /p:Configuration=Release /p:Platform=x64
```

The project must have Common Language Runtime Support (`/clr`) enabled, and reference the launcher's plugin SDK and Avalonia assemblies.

---

## Installing

1. Copy your built `.dll` into:
   ```
   C:\Users\%Username%\AppData\Roaming\com.cassie-launcher\plugins
   ```
2. If your build produces `Ijwhost.dll`, copy that too (see below).
3. Click the reload button in the launcher — your plugin should appear in the plugin list.

---

## Important: Ijwhost.dll and plugin-ignore.json

C++/CLI plugins need `Ijwhost.dll` to bootstrap at runtime. The launcher normally tries to load every `.dll` in the plugins folder as a plugin, so `Ijwhost.dll` needs to be excluded or the launcher will fail trying to load it.

To exclude it:

1. Open (or create):
   ```
   C:\Users\%Username%\AppData\Roaming\com.cassie-launcher\plugin-ignore.json
   ```
2. Add it to the list:
   ```json
   [
     "FileNames": [
        "ijwhost.dll"
     ],
     "Prefixes": []
   ]
   ```
3. Restart the launcher.

> This only applies to C++/CLI plugins — pure C# plugins don't need this step.

---

## Troubleshooting

| Symptom | Likely Cause | Fix |
|---|---|---|
| Plugin doesn't appear in launcher | Wrong folder or build failed | Confirm DLL path and rebuild in Release/x64 |
| Launcher errors mentioning `Ijwhost.dll` | Not excluded | Add `"Ijwhost.dll"` to `plugin-ignore.json` |
| Crash or leak on unload | Native resources not cleaned up | Implement destructor + finalizer pattern |
| Build fails with CLR errors | `/clr` not enabled or wrong platform | Enable CLR support, set Platform to `x64` |