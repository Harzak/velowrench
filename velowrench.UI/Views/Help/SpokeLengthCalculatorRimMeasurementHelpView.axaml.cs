using Avalonia;
using Avalonia.Controls;
using Avalonia.Styling;
using System;
using velowrench.UI.Services;

namespace velowrench.UI.Views.Help;

public partial class SpokeLengthCalculatorRimMeasurementHelpView : UserControl
{
    public SpokeLengthCalculatorRimMeasurementHelpView()
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
        string diagramColor = ThemeBasedResourcesLocator.GetDiagramColor();
        string diagramLegendColor = ThemeBasedResourcesLocator.GetDiagramLegendColor();

        Avalonia.Svg.Skia.Svg.SetCss(RimDiagram,
            $".lineColor {{ fill: {diagramColor}; stroke: {diagramColor}; }}" +
            $".legendColor {{ fill: {diagramLegendColor}; stroke: {diagramLegendColor} }}");

    }
}