using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;
using Avalonia.Styling;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using velowrench.Calculations.Configuration;
using velowrench.Core.Configuration;
using velowrench.Core.Interfaces;
using velowrench.Core.ViewModels;
using velowrench.UI.Configuration;
using velowrench.UI.Resources;
using velowrench.UI.Views;

namespace velowrench.UI;

public partial class App : Application
{
    public static IServiceProvider? ServiceProvider { get; private set; }
    public static Action<IServiceCollection>? RegisterPlatformSpecificServices { get; set; }

    private Window? _mainWindow;
    private WindowIcon? _lightThemeIcon;
    private WindowIcon? _darkThemeIcon;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        
        _lightThemeIcon = new WindowIcon(AssetLoader.Open(new Uri("avares://velowrench.UI/Assets/velowrench-logo-light.ico")));
        _darkThemeIcon = new WindowIcon(AssetLoader.Open(new Uri("avares://velowrench.UI/Assets/velowrench-logo-dark.ico")));
    }

    public override void OnFrameworkInitializationCompleted()
    {
        Languages.Culture = new System.Globalization.CultureInfo("en-US");

        ServiceCollection collection = new();
        collection.AddLogging();
        collection.AddCoreServices();
        collection.AddUIServices();
        collection.AddCalculationServices();
        RegisterPlatformSpecificServices?.Invoke(collection);

        ServiceProvider = collection.BuildServiceProvider();

        var vm = ServiceProvider.GetRequiredService<MainViewModel>();
        var navigationService = ServiceProvider.GetRequiredService<INavigationService>();
        vm.Initialize(navigationService);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            DisableAvaloniaDataAnnotationValidation();
            _mainWindow = new Window
            {
                Height = 640,
                Width = 360,
                Icon = GetIconForCurrentTheme(),
                Content = new MainView()
                {
                    DataContext = vm
                }
            };
            desktop.MainWindow = _mainWindow;
            this.ActualThemeVariantChanged += OnThemeChanged;
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

    private void OnThemeChanged(object? sender, EventArgs e)
    {
        if (_mainWindow != null)
        {
            _mainWindow.Icon = GetIconForCurrentTheme();
        }
    }

    private WindowIcon GetIconForCurrentTheme()
    {
        return this.ActualThemeVariant == ThemeVariant.Dark ? _darkThemeIcon! : _lightThemeIcon!;
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        DataAnnotationsValidationPlugin[] dataValidationPluginsToRemove = [.. BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>()];

        foreach (DataAnnotationsValidationPlugin plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}