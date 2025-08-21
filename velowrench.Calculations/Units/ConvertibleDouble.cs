using UnitsNet;
using UnitsNet.Units;

namespace velowrench.Calculations.Units;

/// <summary>
/// Represents a numeric value with an associated unit that can be converted between different units of the same type.
/// </summary>
/// <typeparam name="T">The enum type representing the unit system (e.g., LengthUnit, VolumeUnit).</typeparam>
public sealed class ConvertibleDouble<T> where T : Enum
{
    private T _unit;
    private double _value;

    /// <summary>
    /// Gets or sets the unit of measurement for this value.
    /// </summary>
    /// <value>
    /// The current unit of measurement. Setting this property automatically converts the value to the new unit.
    /// </value>
    /// <remarks>
    /// When the unit is changed, the numeric value is automatically converted to maintain the same physical quantity.
    /// </remarks>
    public T Unit
    {
        get => _unit;
        set
        {
            if (UnitConverter.TryConvert(Value, _unit, value, out double convertedValue))
            {
                _unit = value;
                Value = convertedValue;
            }
        }
    }

    /// <summary>
    /// Gets or sets the numeric value in the current unit.
    /// </summary>
    /// <value>
    /// The numeric value expressed in the current unit of measurement.
    /// </value>
    /// <remarks>
    /// Setting this property raises the <see cref="ValueChanged"/> event if the value actually changes.
    /// </remarks>
    public double Value
    {
        get => _value;
        set
        {
            if (_value != value)
            {
                _value = value;
                ValueChanged?.Invoke(this, new EventArgs());
            }
        }
    }

    /// <summary>
    /// Event raised when the <see cref="Value"/> property changes.
    /// </summary>
    public EventHandler<EventArgs>? ValueChanged { get; set; }

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
    /// Creates a new instance of <see cref="ConvertibleDouble{T}"/> with a value of zero and the default unit.
    /// </summary>
    /// <returns>A new instance with default values.</returns>
    public static ConvertibleDouble<T> Default()
    {
        return new ConvertibleDouble<T>(0, (T)GetDefault());
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
