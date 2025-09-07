using Android.App;
using Android.Content.PM;
using Android.OS;
using Avalonia;
using Avalonia.Android;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using velowrench.Core.Interfaces;
using velowrench.UI;

namespace velowrench.Shell.Android;

[Activity(
    Label = "velowrench.Android",
    Theme = "@style/MyTheme.NoActionBar",
    Icon = "@mipmap/ic_launcher",
    MainLauncher = true,
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
public class MainActivity : AvaloniaMainActivity<App>
{
    private INavigationService? _navigationService;

    protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
    {
        App.RegisterPlatformSpecificServices = services =>
        {
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddDebug();
                loggingBuilder.SetMinimumLevel(LogLevel.Debug);
                loggingBuilder.AddConsole();
            });
        };

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
            _navigationService.NavigateBackAsync();
        }
        else
        {
            MoveTaskToBack(true);
        }
    }
}