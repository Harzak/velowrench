using Android.App;
using Android.Content.PM;
using Avalonia;
using Avalonia.Android;
using System.Globalization;
using velowrench.Core;
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
    protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
    {
        CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
        CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");
    
        return base.CustomizeAppBuilder(builder)
            .WithInterFont();
    }
}
