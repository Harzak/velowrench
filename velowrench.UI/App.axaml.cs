using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;
using Avalonia.Styling;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using velowrench.Calculations.Configuration;
using velowrench.Core.Configuration;
using velowrench.Core.Interfaces;
using velowrench.Core.ViewModels;
using velowrench.UI.Configuration;
using velowrench.UI.Resources;
using velowrench.UI.Services;
using velowrench.UI.Views;

namespace velowrench.UI;

public partial class App : Application
{
    private Window? _mainWindow;

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
        collection.AddLogging();
        collection.AddCoreServices();
        collection.AddUIServices();
        collection.AddCalculationServices();
        RegisterPlatformSpecificServices?.Invoke(collection);

        ServiceProvider = collection.BuildServiceProvider();

        MainViewModel vm = ServiceProvider.GetRequiredService<MainViewModel>();
        INavigationService navigationService = ServiceProvider.GetRequiredService<INavigationService>();
        IAppConfiguration appConfiguration = ServiceProvider.GetRequiredService<IAppConfiguration>();
        Task.Run(async () =>
        {
            await vm.InitializeAsync(navigationService, appConfiguration).ConfigureAwait(false);
        });

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            DisableAvaloniaDataAnnotationValidation();
            _mainWindow = new Window
            {
                Height = 640,
                Width = 360,
                Icon = new WindowIcon(AssetLoader.Open(ThemeBasedResourcesLocator.GetAppLogoUri())),
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
            _mainWindow.Icon = new WindowIcon(AssetLoader.Open(ThemeBasedResourcesLocator.GetAppLogoUri()));
        }
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