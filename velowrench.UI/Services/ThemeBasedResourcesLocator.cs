using ExCSS;
using SkiaSharp;

namespace velowrench.UI.Services;

public static class ThemeBasedResourcesLocator
{
    private const string APP_LOGO_PATH_LIGHT = "avares://velowrench.UI/Assets/velowrench-logo-light.ico";
    private const string APP_LOGO_PATH_DARK = "avares://velowrench.UI/Assets/velowrench-logo-dark.ico";

    private readonly static SKColor FONT_COLOR_LIGHT = new(28, 31, 35);
    private readonly static SKColor FONT_COLOR_DARK = new(198, 202, 205);

    private const string DIAGRAM_SIDE_VIEW_COLOR_LIGHT = "#1C1F23";
    private const string DIAGRAM_SIDE_VIEW_COLOR_DARK = "#C6CACD";
    private const string DIAGRAM_LEGEND_COLOR_LIGHT = "#0095EE";
    private const string DIAGRAM_LEGEND_COLOR_DARK = "#40B4F3";

    public static Url GetAppLogoUri()
    {
        string path = IsDarkTheme() ? APP_LOGO_PATH_DARK : APP_LOGO_PATH_LIGHT;
        return new Url(path);
    }

    public static SKColor GetSKColorFont()
    {
        return IsDarkTheme() ? FONT_COLOR_DARK : FONT_COLOR_LIGHT;
    }

    public static string GetDiagramColor()
    {
        return IsDarkTheme() ? DIAGRAM_SIDE_VIEW_COLOR_DARK : DIAGRAM_SIDE_VIEW_COLOR_LIGHT;
    }

    public static string GetDiagramLegendColor()
    {
        return IsDarkTheme() ? DIAGRAM_LEGEND_COLOR_DARK : DIAGRAM_LEGEND_COLOR_LIGHT;
    }

    private static bool IsDarkTheme()
    {
        return Avalonia.Application.Current?.ActualThemeVariant == Avalonia.Styling.ThemeVariant.Dark;
    }
}
