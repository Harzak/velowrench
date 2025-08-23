using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Svg.Skia;
using Avalonia.Styling;
using System;
using System.Diagnostics;

namespace velowrench.UI.Views.Help;

public partial class SpokeLengthCalculatorHubMeasurementHelpView : UserControl
{
    public SpokeLengthCalculatorHubMeasurementHelpView()
    {
        InitializeComponent();
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        UpdateSvgColor();
        
        if (Application.Current != null)
        {
            Application.Current.ActualThemeVariantChanged += OnThemeChanged;
        }
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        if (Application.Current != null)
        {
            Application.Current.ActualThemeVariantChanged -= OnThemeChanged;
        }
        base.OnDetachedFromVisualTree(e);
    }

    private void OnThemeChanged(object? sender, EventArgs e)
    {
        UpdateSvgColor();
    }

    private void UpdateSvgColor()
    {
        string diagramColor =  Application.Current?.ActualThemeVariant == ThemeVariant.Dark ? "#C6CACD" : "#1C1F23";
        string legendColor =  Application.Current?.ActualThemeVariant == ThemeVariant.Dark ? "#40B4F3" : "#0095EE";
        Avalonia.Svg.Skia.Svg.SetCss(HubDiagram, $".lineColor {{ fill: {diagramColor}; }} .legendColor {{ fill: {legendColor}; stroke: {legendColor} }}");
    }
}