using UnitsNet.Units;
using velowrench.Calculations.Enums;

namespace velowrench.Calculations.Units;

/// <summary>
/// Provides a centralized store for units used in the app (calculations, user preferences)
/// </summary>
public static class UnitsStore
{
    #region Gear Calculations
    /// <summary>
    /// Gets the collection of length units available for development calculation result.
    /// </summary>
    public static IReadOnlyCollection<LengthUnit> DevelopmentResultAvailableUnits => [
        LengthUnit.Centimeter,
        LengthUnit.Meter,
        LengthUnit.Inch,
        LengthUnit.Foot,
        LengthUnit.Yard
    ];

    /// <summary>
    /// Gets the default unit of measurement for development calculation result.
    /// </summary>
    public static LengthUnit DevelopmentResultDefaultUnit => LengthUnit.Meter;

    /// <summary>
    /// Gets the collection of speed units available for speed calculation result.
    /// </summary>
    public static IReadOnlyCollection<SpeedUnit> SpeedResultAvailableUnits => [
        SpeedUnit.KilometerPerHour,
        SpeedUnit.MilePerHour,
    ];

    /// <summary>
    /// Gets the default unit of measurement for speed calculation result.
    /// </summary>
    public static SpeedUnit SpeedResultDefaultUnit => SpeedUnit.KilometerPerHour;

    /// <summary>
    /// Retrieves the default unit associated with the specified gear calculation type.
    /// </summary>
    /// <returns>
    /// The default unit for the specified gear calculation type, or <see langword="null"/> if no default unit is
    /// defined.
    /// </returns>
    public static Enum? GetDefaultUnitForGearCalculation(EGearCalculatorType type)
    {
        return type switch
        {
            EGearCalculatorType.Development => DevelopmentResultDefaultUnit,
            EGearCalculatorType.Speed => SpeedResultDefaultUnit,
            EGearCalculatorType.GearInches => null,
            EGearCalculatorType.GainRatio => null,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Unsupported gear calculation type")
        };
    }

    /// <summary>
    /// Retrieves the collection of available units for the specified gear calculation type.
    /// </summary>
    public static IReadOnlyCollection<Enum> GetAvailableUnitForGearCalculation(EGearCalculatorType type)
    {
        return type switch
        {
            EGearCalculatorType.Development => [.. DevelopmentResultAvailableUnits.Cast<Enum>()],
            EGearCalculatorType.Speed => [.. SpeedResultAvailableUnits.Cast<Enum>()],
            EGearCalculatorType.GearInches => [],
            EGearCalculatorType.GainRatio => [],
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Unsupported gear calculation type")
        };
    }

    #endregion

    #region Wheel Calculations
    public static IReadOnlyCollection<LengthUnit> WheelMeasurementsAvailableUnits => [
        LengthUnit.Millimeter,
        LengthUnit.Centimeter,
        LengthUnit.Inch
    ];

    public static LengthUnit WheelMeasurementsDefaultUnit => WheelMeasurementsAvailableUnits.First();
    #endregion
}
