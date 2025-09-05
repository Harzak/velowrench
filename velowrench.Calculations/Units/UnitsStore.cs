using UnitsNet.Units;
using velowrench.Calculations.Enums;
using velowrench.Calculations.Interfaces;

namespace velowrench.Calculations.Units;

/// <summary>
/// Provides a centralized store for units used in the app (default, supported, user preferred units)
/// </summary>
public sealed class UnitStore : IUnitStore
{
    private LengthUnit _lengthDefaultUnit;
    private LengthUnit _distanceDefaultUnit;
    private SpeedUnit _speedDefaultUnit;

    /// <inheritdoc/>
    public LengthUnit LengthDefaultUnit
    {
        get => _lengthDefaultUnit;
        set
        {
            if (_lengthDefaultUnit != value)
            {
                _lengthDefaultUnit = this.LengthAvailableUnits.Contains(value) ? value 
                    : throw new InvalidOperationException("Unsupported unit");
            }
        }
    }
    /// <inheritdoc/>
    public IReadOnlyCollection<LengthUnit> LengthAvailableUnits => [
        LengthUnit.Millimeter,
        LengthUnit.Centimeter,
        LengthUnit.Inch
    ];

    /// <inheritdoc/>
    public LengthUnit DistanceDefaultUnit
    {
        get => _distanceDefaultUnit;
        set
        {
            if (_distanceDefaultUnit != value)
            {
                _distanceDefaultUnit = this.DistanceAvailableUnits.Contains(value) ? value
                    : throw new InvalidOperationException("Unsupported unit");
            }
        }
    }
    /// <inheritdoc/>
    public IReadOnlyCollection<LengthUnit> DistanceAvailableUnits => [
        LengthUnit.Meter,
        LengthUnit.Kilometer,
        LengthUnit.Inch,
        LengthUnit.Foot,
        LengthUnit.Yard
    ];

    /// <inheritdoc/>
    public SpeedUnit SpeedDefaultUnit
    {
        get => _speedDefaultUnit;
        set
        {
            if (_speedDefaultUnit != value)
            {
                _speedDefaultUnit = this.SpeedAvailableUnits.Contains(value) ? value
                    : throw new InvalidOperationException("Unsupported unit");
            }
        }
    }
    /// <inheritdoc/>
    public IReadOnlyCollection<SpeedUnit> SpeedAvailableUnits => [
        SpeedUnit.KilometerPerHour,
        SpeedUnit.MilePerHour,
    ];

    public UnitStore()
    {
        _lengthDefaultUnit =  LengthUnit.Millimeter;
        _distanceDefaultUnit = LengthUnit.Meter;
        _speedDefaultUnit = SpeedUnit.KilometerPerHour;
    }

    /// <inheritdoc/>
    public Enum? GetDefaultUnitForGearCalculation(EGearCalculatorType type)
    {
        return type switch
        {
            EGearCalculatorType.Development => this.DistanceDefaultUnit,
            EGearCalculatorType.Speed => this.SpeedDefaultUnit,
            EGearCalculatorType.GearInches => null,
            EGearCalculatorType.GainRatio => null,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Unsupported gear calculation type")
        };
    }

    /// <inheritdoc/>
    public IReadOnlyCollection<Enum> GetAvailableUnitForGearCalculation(EGearCalculatorType type)
    {
        return type switch
        {
            EGearCalculatorType.Development => [.. this.DistanceAvailableUnits.Cast<Enum>()],
            EGearCalculatorType.Speed => [.. this.SpeedAvailableUnits.Cast<Enum>()],
            EGearCalculatorType.GearInches => [],
            EGearCalculatorType.GainRatio => [],
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Unsupported gear calculation type")
        };
    }
}