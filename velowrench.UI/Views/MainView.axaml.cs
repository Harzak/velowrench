using Avalonia;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;
using velowrench.UI.Services;

namespace velowrench.UI.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);

        this.UpdateAppLogo();

        if (Application.Current != null)
        {
            Application.Current.ActualThemeVariantChanged += OnThemeChanged;
        }
    }

    private void UpdateAppLogo()
    {
        logo.Source = new Bitmap(AssetLoader.Open(ThemeBasedResourcesLocator.GetAppLogoUri()));
    }

    private void OnThemeChanged(object? sender, EventArgs e)
    {
        this.UpdateAppLogo();
    }
}