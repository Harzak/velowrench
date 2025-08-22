using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnitsNet.Units;
using velowrench.Core.Units;

namespace velowrench.UI.Controls;

public partial class LengthTextBlock : UserControl, INotifyPropertyChanged
{
    private readonly List<LengthUnit> _availableUnits =
    [
        LengthUnit.Millimeter,
        LengthUnit.Centimeter,
        LengthUnit.Inch
    ];

    public static readonly StyledProperty<string> FormatProperty =
        AvaloniaProperty.Register<LengthUpDown, string>(nameof(Format), string.Empty);

    public static readonly StyledProperty<int> TextFontSizeProperty =
        AvaloniaProperty.Register<LengthUpDown, int>(nameof(TextFontSize), 14);

    public static readonly StyledProperty<ConvertibleDouble<LengthUnit>?> ValueProperty =
        AvaloniaProperty.Register<LengthUpDown, ConvertibleDouble<LengthUnit>?>(
            nameof(Value),
            defaultBindingMode: BindingMode.OneWay);

    public string Format
    {
        get => GetValue(FormatProperty);
        set => SetValue(FormatProperty, value);
    }

    public int TextFontSize
    {
        get => GetValue(TextFontSizeProperty);
        set => SetValue(TextFontSizeProperty, value);
    }

    public ConvertibleDouble<LengthUnit>? Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public List<LengthUnit> AvailableUnits => _availableUnits;

    public LengthUnit SelectedUnit
    {
        get => Value?.Unit ?? LengthUnit.Centimeter;
        set
        {
            if (Value != null && Value.Unit != value)
            {
                Value.Unit = value;
                RaisePropertyChanged(nameof(SelectedUnit));
                RaisePropertyChanged(nameof(DisplayValue));
            }
        }
    }

    public string DisplayValue
    {
        get => Value?.Value.ToString(Format) ?? "0";
    }

    public new event PropertyChangedEventHandler? PropertyChanged;

    public LengthTextBlock()
    {
        InitializeComponent();
        Value ??= new ConvertibleDouble<LengthUnit>(0, LengthUnit.Centimeter);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == ValueProperty)
        {
            if (change.OldValue is ConvertibleDouble<LengthUnit> oldValue)
            {
                oldValue.PropertyChanged -= OnConvertibleDoubleValueChanged;
            }

            if (change.NewValue is ConvertibleDouble<LengthUnit> newValue)
            {
                newValue.PropertyChanged += OnConvertibleDoubleValueChanged;
            }

            RaisePropertyChanged(nameof(SelectedUnit));
            RaisePropertyChanged(nameof(DisplayValue));
        }
    }

    protected void RaisePropertyChanged(string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void OnConvertibleDoubleValueChanged(object? sender, EventArgs e)
    {
        RaisePropertyChanged(nameof(DisplayValue));
    }
}