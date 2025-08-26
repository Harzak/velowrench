using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnitsNet.Units;
using velowrench.Core.Units;

namespace velowrench.UI.Controls;

public partial class LengthUpDownControl : UserControl, INotifyPropertyChanged
{
    private readonly List<LengthUnit> _availableUnits =
    [
        LengthUnit.Millimeter,
        LengthUnit.Centimeter,
        LengthUnit.Inch
    ];

    public static readonly StyledProperty<string> TitleProperty =
        AvaloniaProperty.Register<LengthUpDownControl, string>(nameof(Title), string.Empty);

    public static readonly StyledProperty<ConvertibleDouble<LengthUnit>?> ConvertibleDoubleProperty =
        AvaloniaProperty.Register<LengthUpDownControl, ConvertibleDouble<LengthUnit>?>(
            nameof(ConvertibleDouble),
            defaultBindingMode: BindingMode.TwoWay);

    public string Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public ConvertibleDouble<LengthUnit>? ConvertibleDouble
    {
        get => GetValue(ConvertibleDoubleProperty);
        set => SetValue(ConvertibleDoubleProperty, value);
    }

    public List<LengthUnit> AvailableUnits => _availableUnits;

    public LengthUnit SelectedUnit
    {
        get => ConvertibleDouble?.Unit ?? LengthUnit.Centimeter;
        set
        {
            if (ConvertibleDouble != null && ConvertibleDouble.Unit != value)
            {
                ConvertibleDouble.Unit = value;
                RaisePropertyChanged(nameof(SelectedUnit));
                RaisePropertyChanged(nameof(DisplayValue));
            }
        }
    }

    public decimal? DisplayValue
    {
        get => (decimal?)ConvertibleDouble?.Value ?? 0;
        set
        {
            if (value.HasValue)
            {
                double convertedValue = (double)value;
                if (ConvertibleDouble != null && ConvertibleDouble.Value != convertedValue  && ConvertibleDouble.Value != double.NaN)
                {

                    ConvertibleDouble.Value =convertedValue;
                    RaisePropertyChanged(nameof(DisplayValue));
                }
            }
            else
            {
                ConvertibleDouble = null;
            }
        }
    }

    public new event PropertyChangedEventHandler? PropertyChanged;

    public LengthUpDownControl()
    {
        InitializeComponent();
        ConvertibleDouble ??= new ConvertibleDouble<LengthUnit>(0, LengthUnit.Centimeter);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == ConvertibleDoubleProperty)
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