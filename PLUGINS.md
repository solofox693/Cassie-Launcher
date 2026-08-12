# Writing a plugin

Plugins are plain .NET class libraries that implement `IFnPlugin`
(`Launcher-main/Plugins/IFnPlugin.cs`) and get built into a `.dll`. Drop the
`.dll` into the launcher's `plugins` folder — next to the exe — and it shows
up as a card under **Authentication → Plugins**. Hit **Reload** on that tab
(or just restart the launcher) to pick up new/changed DLLs.

## The contract

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

Implement this on a **public class with a public parameterless constructor**.
`CreateView()` is called once, the first time your card is opened, and the
returned control is cached and reused after that — so it's fine to keep state
in fields on your plugin/view, a button counter, cached data, etc.

## Setting up your project

See `ExamplePlugin/` for a complete working example (`HelloPlugin.cs` +
`ExamplePlugin.csproj`) — it's a plain class library, no Avalonia XAML
project template needed, since building the UI in code (as the example does)
is enough.

The one thing that matters in your `.csproj`: reference the host project like
this, not as a normal reference:

```xml
<ItemGroup>
    <ProjectReference Include="..\Launcher-main\MinimalFortnitePortingUI.csproj">
        <Private>false</Private>
        <ExcludeAssets>runtime</ExcludeAssets>
    </ProjectReference>
</ItemGroup>
```

`Private=false` + `ExcludeAssets=runtime` means you get `IFnPlugin` and
Avalonia's types at compile time, but your plugin's own build output does
**not** end up with a copy of `MinimalFortnitePortingUI.dll` (or the Avalonia
DLLs) sitting next to it.

That copy is the one thing that will actually break your plugin: the
launcher loads each plugin DLL into its own isolated
`AssemblyLoadContext` so one broken/incompatible plugin can't take the whole
app down. If your plugin's output folder also contains its own copy of
`MinimalFortnitePortingUI.dll`, that copy gets loaded into your plugin's
context instead of reusing the launcher's already-running one — and at that
point your plugin's `IFnPlugin` is technically a *different type* to the
launcher's, even though the code is identical. Your class will silently fail
the "does this implement IFnPlugin" check and show up as **Failed to load**
on the Plugins tab.

If your plugin needs its own extra dependencies (a NuGet package the host
doesn't already have), that's fine — just build normally and copy the whole
output folder's DLLs (minus `MinimalFortnitePortingUI.dll` itself) into
`plugins/`. The loader resolves a plugin's own dependencies from whatever
sits next to its DLL.

## Building and testing

```
cd ExamplePlugin
dotnet build
copy bin\Debug\net8.0\ExamplePlugin.dll <path-to-launcher>\plugins\
```

Then launch the app (or hit Reload on the Plugins tab) and open the
"Hello Plugin" card.

## If a plugin fails to load

The Plugins tab shows every DLL found in the folder, loaded or not. A failed
one shows a **Failed** badge with the reason in place of its description —
usually one of:

- no public class implementing `IFnPlugin` with a parameterless constructor
- an exception thrown while constructing your plugin instance
- a missing dependency DLL that wasn't copied alongside your plugin's DLL
