using CommunityToolkit.Mvvm.ComponentModel;
using UnitsNet;
using UnitsNet.Units;

namespace velowrench.Core.Units;

/// <summary>
/// Represents a numeric value with an associated unit that can be converted between different units of the same type.
/// </summary>
/// <typeparam name="T">The enum type representing the unit system (e.g., LengthUnit, VolumeUnit).</typeparam>
public  sealed partial class ConvertibleDouble<T> : ObservableObject where T : Enum
{
    private readonly Action<double>? _onValueChanged;

    /// <summary>
    /// Gets or sets the unit of measurement for this value.
    /// </summary>
    /// <value>
    /// The current unit of measurement. Setting this property automatically converts the value to the new unit.
    /// </value>
    /// <remarks>
    /// When the unit is changed, the numeric value is automatically converted to maintain the same physical quantity.
    /// </remarks>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Value))]
    private T _unit;

    /// <summary>
    /// Gets or sets the numeric value in the current unit.
    /// </summary>
    /// <value>
    /// The numeric value expressed in the current unit of measurement.
    /// </value>
    [ObservableProperty]
    private double _value;

    public ConvertibleDouble()
    {
        this.Value = 0;
        _unit = (T)GetDefault();
    }

    public ConvertibleDouble(double value) : this()
    {
        this.Value = value;
    }

    public ConvertibleDouble(double value, T unit) : this(value)
    {
        _unit = unit;
    }

    public ConvertibleDouble(double value, T unit, Action<double> OnValueChanged) : this(value, unit)
    {
        _onValueChanged = OnValueChanged;
    }

    partial void OnValueChanged(double value)
    {
        _onValueChanged?.Invoke(value);
    }

    partial void OnUnitChanging(T? oldValue, T newValue)
    {
        if (UnitConverter.TryConvert(Value, _unit, newValue, out double convertedValue))
        {
            _unit = newValue;
            Value = convertedValue;
        }
    }

    /// <summary>
    /// Gets the current value converted to the specified unit.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the conversion between units is not possible.</exception>
    public double GetValueIn(T unit)
    {
        if (UnitConverter.TryConvert(Value, Unit, unit, out double convertedValue))
        {
            return convertedValue;
        }
        throw new InvalidOperationException($"Cannot convert from {Unit} to {unit}");
    }

    /// <summary>
    /// Gets the default base unit for the specified unit type.
    /// </summary>
    /// <exception cref="NotImplementedException">Thrown when the unit type is not supported.</exception>
    private static Enum GetDefault() => typeof(T) switch
    {
        Type type when type == typeof(LengthUnit) => Length.BaseUnit,
        Type type when type == typeof(VolumeUnit) => Volume.BaseUnit,
        Type type when type == typeof(MassUnit) => Mass.BaseUnit,
        Type type when type == typeof(DensityUnit) => Density.BaseUnit,
        _ => throw new NotImplementedException(),
    };
}
