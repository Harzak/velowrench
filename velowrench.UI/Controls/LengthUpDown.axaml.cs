using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnitsNet.Units;
using velowrench.Core.Units;

namespace velowrench.UI.Controls;

public partial class LengthUpDown : UserControl, INotifyPropertyChanged
{
    private readonly List<LengthUnit> _availableUnits =
    [
        LengthUnit.Millimeter,
        LengthUnit.Centimeter,
        LengthUnit.Inch
    ];

    public static readonly StyledProperty<string> TitleProperty =
        AvaloniaProperty.Register<LengthUpDown, string>(nameof(Title), string.Empty);

    public static readonly StyledProperty<double> MinimumProperty =
        AvaloniaProperty.Register<LengthUpDown, double>(nameof(Minimum), 0.0);

    public static readonly StyledProperty<double> MaximumProperty =
        AvaloniaProperty.Register<LengthUpDown, double>(nameof(Maximum), 1000.0);

    public static readonly StyledProperty<double> IncrementProperty =
        AvaloniaProperty.Register<LengthUpDown, double>(nameof(Increment), 1.0);

    public static readonly StyledProperty<ConvertibleDouble<LengthUnit>?> ValueProperty =
        AvaloniaProperty.Register<LengthUpDown, ConvertibleDouble<LengthUnit>?>(
            nameof(Value),
            defaultBindingMode: BindingMode.TwoWay);

    public string Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public double Minimum
    {
        get => GetValue(MinimumProperty);
        set => SetValue(MinimumProperty, value);
    }

    public double Maximum
    {
        get => GetValue(MaximumProperty);
        set => SetValue(MaximumProperty, value);
    }

    public double Increment
    {
        get => GetValue(IncrementProperty);
        set => SetValue(IncrementProperty, value);
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

    public double DisplayValue
    {
        get => Value?.Value ?? 0;
        set
        {
            if (Value != null && Value.Value != value)
            {
                Value.Value = value;
                RaisePropertyChanged(nameof(DisplayValue));
            }
        }
    }

    public new event PropertyChangedEventHandler? PropertyChanged;

    public LengthUpDown()
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
                oldValue.ValueChanged -= OnConvertibleDoubleValueChanged;
            }

            if (change.NewValue is ConvertibleDouble<LengthUnit> newValue)
            {
                newValue.ValueChanged += OnConvertibleDoubleValueChanged;
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