using System;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using CassieLauncher.Plugins;

namespace MyPlugin;

public class Plugin : IFnPlugin
{
    public string Name => "Example Plugin";
    public string Version => "1.0.0";
    public string Author => "YourName";
    public string Description => "A sample plugin for Cassie Launcher.";

    public Control CreateView()
    {
        return new StackPanel
        {
            Margin = new Avalonia.Thickness(20),
            Spacing = 10,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Children =
            {
                new TextBlock
                {
                    Text = "Hello from Example Plugin!",
                    FontSize = 20,
                    FontWeight = FontWeight.Bold,
                    Foreground = Brushes.White
                },
                new Button
                {
                    Content = "Click Me",
                    HorizontalAlignment = HorizontalAlignment.Center
                }
            }
        };
    }
}