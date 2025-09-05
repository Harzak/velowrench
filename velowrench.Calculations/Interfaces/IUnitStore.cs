using UnitsNet.Units;
using velowrench.Calculations.Enums;

namespace velowrench.Calculations.Interfaces;

/// <summary>
/// Represents a store for default, user preferred and available units of measurement
/// </summary>
public interface IUnitStore
{
    /// <summary>
    /// Gets or sets the default unit of measurement for length values (for measurement of hub, rim etc.)
    /// </summary>
    LengthUnit LengthDefaultUnit { get; set; }

    /// <summary>
    /// Gets the collection of length units that are available for use in the application.
    /// </summary>
    IReadOnlyCollection<LengthUnit> LengthAvailableUnits { get; }

    /// <summary>
    /// Gets or sets the default unit of measurement for distance values (for measurement of traveled distances etc.)
    /// </summary>
    LengthUnit DistanceDefaultUnit { get; set; }

    /// <summary>
    /// Gets the collection of distance units that are available for use in the application.
    /// </summary>
    IReadOnlyCollection<LengthUnit> DistanceAvailableUnits { get; }

    /// <summary>
    /// Gets or sets the default unit of measurement for speed values (for measurement of cycling speed etc.)
    /// </summary>
    SpeedUnit SpeedDefaultUnit { get; set; }

    /// <summary>
    /// Gets the collection of speed units available for use in the application.
    /// </summary>
    IReadOnlyCollection<SpeedUnit> SpeedAvailableUnits { get; }

    /// <summary>
    /// Retrieves the default unit of measurement used for gear calculations based on the specified gear calculator type.
    /// </summary>
    Enum? GetDefaultUnitForGearCalculation(EGearCalculatorType type);

    /// <summary>
    /// Retrieves the collection of available units for gear calculation based on the specified calculator type.
    /// </summary>
    IReadOnlyCollection<Enum> GetAvailableUnitForGearCalculation(EGearCalculatorType type);
}