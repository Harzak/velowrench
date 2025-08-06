using Android.App;
using Android.Content.PM;
using Android.Content;
using Android.OS;
using Avalonia;
using Avalonia.Android;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using velowrench.Core.Configuration;
using velowrench.Core.ViewModels;
using velowrench.Shell.Android.Activities;
using velowrench.UI;
using System;
using velowrench.Core.Services;

namespace velowrench.Shell.Android;

[Activity(
    Label = "velowrench.Android",
    Theme = "@style/MyTheme.NoActionBar",
    Icon = "@drawable/icon",
    MainLauncher = true,
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
public class MainActivity : BaseAvaloniaActivity<MainViewModel>
{
    private static IServiceProvider? _serviceProvider;
    private INavigationService? _navigationService;

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        // Set up culture
        CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
        CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");

        // Initialize Avalonia if not already done
        if (_serviceProvider == null)
        {
            InitializeAvalonia();
        }

        base.OnCreate(savedInstanceState);
    }

    private void InitializeAvalonia()
    {
        // Initialize Avalonia but don't start it (we'll manually create what we need)
        AppBuilder.Configure<App>()
            .UseAndroid()
            .WithInterFont()
            .SetupWithoutStarting();
    }

    protected override MainViewModel CreateViewModel()
    {
        // Use the same DI setup as App.axaml.cs but with Android-specific navigation
        var services = new ServiceCollection();
        services.AddCoreServices();
        services.AddUIServices();
        
        _serviceProvider = services.BuildServiceProvider();
        
        // Get the navigation service for back handling
        _navigationService = _serviceProvider.GetRequiredService<INavigationService>();

        // Create MainViewModel
        return _serviceProvider.GetRequiredService<MainViewModel>();
    }

    public override void OnBackPressed()
    {
        // Try to navigate back in the app navigation stack first
        if (_navigationService?.CanNavigateBack == true)
        {
            _navigationService.NavigateBack();
        }
        else
        {
            // If can't navigate back in app, minimize instead of closing
            MoveTaskToBack(true);
        }
    }
}
