using Avalonia.Controls;
using Avalonia.Layout;
using CassieLauncher.Plugins;

namespace ExamplePlugin;

/// <summary>
/// Minimal example plugin. Build this project, then copy
/// ExamplePlugin.dll into the launcher's "plugins" folder (next to the exe)
/// and hit Reload on the Plugins tab.
/// </summary>
public class HelloPlugin : IFnPlugin
{
    public string Name => "Hello Plugin";
    public string Version => "1.0.0";
    public string Author => "I Love Downies";
    public string Description => "A minimal example plugin — shows a click counter.";

    public Control CreateView()
    {
        var countText = new TextBlock { Text = "Clicked 0 times", FontSize = 14 };

        var button = new Button { Content = "Click me" };
        int count = 0;
        button.Click += (_, _) =>
        {
            count++;
            countText.Text = $"Clicked {count} times";
        };

        return new StackPanel
        {
            Spacing = 12,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Children = { countText, button }
        };
    }
}
