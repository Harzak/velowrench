using Avalonia;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using SkiaSharp;
using System;
using velowrench.UI.Services;

namespace velowrench.UI.Controls;

public partial class LaTeXBitmapControl : UserControl
{
    private const float SCALE_FACTOR = 1.6f;

    public static readonly StyledProperty<string?> FormulaProperty =
        AvaloniaProperty.Register<LaTeXBitmapControl, string?>(nameof(Formula), string.Empty);

    public string? Formula
    {
        get => GetValue(FormulaProperty);
        set => SetValue(FormulaProperty, value);
    }

    public int RenderWidth => (int)(base.Width * SCALE_FACTOR);
    public int RenderHeight => (int)(base.Height * SCALE_FACTOR);

    public LaTeXBitmapControl()
    {
        InitializeComponent();
        base.Width = 200;
        base.Height = 50;
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);

        if (!string.IsNullOrEmpty(this.Formula))
        {
            this.LaTexImage.Source = this.CreateLaTeXBitmap();
        }
    }

    public WriteableBitmap CreateLaTeXBitmap()
    {
        WriteableBitmap bitmap = new(new PixelSize(this.RenderWidth, this.RenderHeight),
                                      new Vector(96, 96),
                                      PixelFormat.Rgba8888,
                                      AlphaFormat.Premul);

        var painter = new CSharpMath.SkiaSharp.MathPainter
        {
            AntiAlias = true,
            LaTeX = this.Formula!,
            FontSize = (float)base.FontSize * SCALE_FACTOR,
            TextColor = ThemeBasedResourcesLocator.GetSKColorFont()
        };

        var bounds = painter.Measure();

        float scaleX = this.RenderWidth/ bounds.Width;
        float scaleY = this.RenderHeight / bounds.Height;

        float scale = Math.Min(scaleX, scaleY);

        painter.FontSize *= scale;

        bounds = painter.Measure();

        using (ILockedFramebuffer lockedBitmap = bitmap.Lock())
        {
            SKImageInfo info = new(this.RenderWidth, this.RenderHeight, SKColorType.Rgba8888, SKAlphaType.Premul);
            using var surface = SKSurface.Create(info, lockedBitmap.Address, lockedBitmap.RowBytes);
            surface.Canvas.Clear(SKColors.Transparent);
            painter.Draw(surface.Canvas, 0, bounds.Height);

        }
        return bitmap;
    }
}