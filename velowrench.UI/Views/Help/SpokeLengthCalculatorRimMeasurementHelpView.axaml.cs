using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using System;

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
        string diagramSideViewColor = Application.Current?.ActualThemeVariant == ThemeVariant.Dark ? "#C6CACD" : "#1C1F23";
        string diagramSideViewLegendColor = Application.Current?.ActualThemeVariant == ThemeVariant.Dark ? "#40B4F3" : "#0095EE";
        Avalonia.Svg.Skia.Svg.SetCss(RimDiagram,
            $".lineColor {{ fill: {diagramSideViewColor}; stroke: {diagramSideViewColor}; }}" +
            $".legendColor {{ fill: {diagramSideViewLegendColor}; stroke: {diagramSideViewLegendColor} }}");

    }
}