using UnitsNet;
using UnitsNet.Units;

namespace velowrench.Core.Units;

public sealed class ConvertibleDouble<T> where T : Enum
{
    private T _unit;
    private double _value;

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

    public double Value
    {
        get => _value;
        set
        {
            _value = value;
            ValueChanged?.Invoke(this, new EventArgs());
        }
    }

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

    public double GetValueIn(T unit)
    {
        if (UnitConverter.TryConvert(Value, Unit, unit, out double convertedValue))
        {
            return convertedValue;
        }
        throw new InvalidOperationException($"Cannot convert from {Unit} to {unit}");
    }

    public static ConvertibleDouble<T> Default()
    {
        return new ConvertibleDouble<T>(0, (T)GetDefault());
    }

    private static Enum GetDefault() => typeof(T) switch
    {
        Type type when type == typeof(LengthUnit) => Length.BaseUnit,
        Type type when type == typeof(VolumeUnit) => Volume.BaseUnit,
        Type type when type == typeof(MassUnit) => Mass.BaseUnit,
        Type type when type == typeof(DensityUnit) => Density.BaseUnit,
        _ => throw new NotImplementedException(),
    };
}
