using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitsNet.Units;
using velowrench.Calculations.Units;

namespace velowrench.Calculations.Calculators.Wheels.SpokeLength;

/// <summary>
/// Represents the input parameters required for calculating spoke lengths in a wheel-building context.
/// </summary>
public record SpokeLengthCalculatorInput
{
    /// <summary>
    /// Gets the distance from the hub center to the flange on the gear (right) side.
    /// </summary>
    public required ConvertibleDouble<LengthUnit> HubCenterToFlangeDistanceGearSide { get; init; }

    /// <summary>
    /// Gets the distance from the hub center to the flange on the non-gear (left) side.
    /// </summary>
    public required ConvertibleDouble<LengthUnit> HubCenterToFlangeDistanceNonGearSide { get; init; }

    /// <summary>
    /// Gets the diameter of the circle through the centers of the spoke holes on the gear side (right) flange.
    /// </summary>
    public required ConvertibleDouble<LengthUnit> HubFlangeDiameterGearSide { get; init; }

    /// <summary>
    /// Gets the diameter of the circle through the centers of the spoke holes on the non-gear side (left) flange.
    /// </summary>
    public required ConvertibleDouble<LengthUnit> HubFlangeDiameterNonGearSide { get; init; }

    /// <summary>
    /// Gets the internal diameter / effective Rim Diameter (ERD) of the rim. The diameter measured at the nipple seats inside the rim
    /// </summary>
    public required ConvertibleDouble<LengthUnit> RimInternalDiameter { get; init; }

    /// <summary>
    /// Gets the number of spokes in the wheel (hole count).
    /// </summary>
    public required int SpokeCount { get; init; }

    /// <summary>
    /// Gets the spoke lacing pattern (cross count). How many times each spoke crosses others (e.g., 3-cross, 2-cross, radial).
    /// </summary>
    public required int SpokeLacingPattern { get; init; }
}

