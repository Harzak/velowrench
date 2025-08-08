using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using velowrench.Calculations.Configuration;
using velowrench.Core.Configuration;
using velowrench.Core.ViewModels;
using velowrench.UI.Configuration;
using velowrench.UI.Resources;
using velowrench.UI.Views;

namespace velowrench.UI;

public partial class App : Application
{
    public static IServiceProvider? ServiceProvider { get; private set; }
    public static Action<IServiceCollection>? RegisterPlatformSpecificServices { get; set; }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        Languages.Culture = new System.Globalization.CultureInfo("en-US");

        ServiceCollection collection = new();
        collection.AddCoreServices();
        collection.AddUIServices();
        collection.AddCalculationServices();
        RegisterPlatformSpecificServices?.Invoke(collection);

        ServiceProvider = collection.BuildServiceProvider();

        var vm = ServiceProvider.GetRequiredService<MainViewModel>();
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            DisableAvaloniaDataAnnotationValidation();
            desktop.MainWindow = new Window
            {
                Height = 640,
                Width = 360,
                Content = new MainView()
                {
                    DataContext = vm
                }
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView()
            {
                DataContext = vm
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        DataAnnotationsValidationPlugin[] dataValidationPluginsToRemove = BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        foreach (DataAnnotationsValidationPlugin plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}