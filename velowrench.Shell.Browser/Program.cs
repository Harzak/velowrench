﻿using Avalonia;
using Avalonia.Browser;
using Avalonia.ReactiveUI;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using velowrench;
using velowrench.Core;
using velowrench.UI;

internal sealed partial class Program
{
    private static Task Main(string[] args) => BuildAvaloniaApp()
            .WithInterFont()
            .StartBrowserAppAsync("out");

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UseReactiveUI();
}