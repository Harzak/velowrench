using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Window;
using Avalonia;
using Avalonia.Android;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using velowrench.Core;
using velowrench.Core.Configuration;
using velowrench.Core.Services;
using velowrench.UI;

namespace velowrench.Shell.Android;

[Activity(
    Label = "velowrench.Android",
    Theme = "@style/MyTheme.NoActionBar",
    Icon = "@drawable/icon",
    MainLauncher = true,
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
public class MainActivity : AvaloniaMainActivity<App>
{
    private static IServiceProvider? _serviceProvider;
    private INavigationService? _navigationService;

    protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
    {
        CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
        CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");

        return base.CustomizeAppBuilder(builder)
            .WithInterFont();
    }

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        _navigationService = App.ServiceProvider?.GetService<INavigationService>();
    }

    public override void OnBackPressed()
    {
        if (_navigationService?.CanNavigateBack == true)
        {
            _navigationService.NavigateBack();
        }
        else
        {
            MoveTaskToBack(true);
        }
    }
}