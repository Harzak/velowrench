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
        UpdateDiagramsColor();
        
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
        UpdateDiagramsColor();
    }

    private void UpdateDiagramsColor()
    {
        string diagramSideViewColor = Application.Current?.ActualThemeVariant == ThemeVariant.Dark ? "#C6CACD" : "#1C1F23";
        string diagramSideViewLegendColor = Application.Current?.ActualThemeVariant == ThemeVariant.Dark ? "#40B4F3" : "#0095EE";
        Avalonia.Svg.Skia.Svg.SetCss(HubDiagramSideView,
            $".lineColor {{ fill: {diagramSideViewColor}; stroke: {diagramSideViewColor}}}" +
            $".legendColor {{ fill: {diagramSideViewLegendColor}; stroke: {diagramSideViewLegendColor} }}");

        string diagramFrontViewColor = Application.Current?.ActualThemeVariant == ThemeVariant.Dark ? "#C6CACD" : "#1C1F23";
        string diagramFrontViewLegendColor = Application.Current?.ActualThemeVariant == ThemeVariant.Dark ? "#40B4F3" : "#0095EE";
        Avalonia.Svg.Skia.Svg.SetCss(HubDiagramFrontView,
            $".lineColor {{ fill: {diagramFrontViewColor}; stroke: {diagramFrontViewColor}; }}" +
            $".legendColor {{ fill: {diagramSideViewLegendColor}; stroke: {diagramFrontViewLegendColor}; }}");
    }
}