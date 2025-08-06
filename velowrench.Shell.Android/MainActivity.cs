using Android.App;
using Android.Content.PM;
using Android.OS;
using Avalonia;
using Avalonia.Android;
using Microsoft.Extensions.DependencyInjection;
using velowrench.Core.Interfaces;
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
    private INavigationService? _navigationService;

    protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
    {
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