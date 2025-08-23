using Avalonia;
using Avalonia.Svg.Skia;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;
using velowrench.UI;

namespace velowrench.Shell.Desktop;

sealed class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        App.RegisterPlatformSpecificServices = services =>
        {
            services.AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                builder.AddNLog();
            });
        };
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    public static AppBuilder BuildAvaloniaApp()
    {
        return AppBuilder.Configure<App>()
                    .UsePlatformDetect()
                    .WithInterFont()
                    .LogToTrace();
    }
}
