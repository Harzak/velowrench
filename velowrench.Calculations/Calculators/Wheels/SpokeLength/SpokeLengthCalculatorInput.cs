using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitsNet.Units;
using velowrench.Calculations.Calculators.Transmission.Chain;
using velowrench.Calculations.Units;

namespace velowrench.Calculations.Calculators.Wheels.SpokeLength;

/// <summary>
/// Represents the input parameters required for calculating spoke lengths in a wheel-building context.
/// </summary>
public sealed record SpokeLengthCalculatorInput
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

    /// <summary>
    /// Custom equality implementation with floating-point tolerance.
    /// </summary>
    public bool Equals(SpokeLengthCalculatorInput? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;

        const double tolerance = 1e-10;

        bool sameCenterToFlangeDistanceGearSide = Math.Abs(HubCenterToFlangeDistanceGearSide.GetValueIn(LengthUnit.Centimeter) - other.HubCenterToFlangeDistanceGearSide.GetValueIn(LengthUnit.Centimeter)) < tolerance;
        bool sameCenterToFlangeDistanceNonGearSide = Math.Abs(HubCenterToFlangeDistanceNonGearSide.GetValueIn(LengthUnit.Centimeter) - other.HubCenterToFlangeDistanceNonGearSide.GetValueIn(LengthUnit.Centimeter)) < tolerance;
        bool sameFlangeDiameterGearSide = Math.Abs(HubFlangeDiameterGearSide.GetValueIn(LengthUnit.Centimeter) - other.HubFlangeDiameterGearSide.GetValueIn(LengthUnit.Centimeter)) < tolerance;
        bool sameFlangeDiameterNonGearSide = Math.Abs(HubFlangeDiameterNonGearSide.GetValueIn(LengthUnit.Centimeter) - other.HubFlangeDiameterNonGearSide.GetValueIn(LengthUnit.Centimeter)) < tolerance;
        bool sameRimInternalDiameter = Math.Abs(RimInternalDiameter.GetValueIn(LengthUnit.Centimeter) - other.RimInternalDiameter.GetValueIn(LengthUnit.Centimeter)) < tolerance;
        return sameCenterToFlangeDistanceGearSide
            && sameCenterToFlangeDistanceNonGearSide
            && sameFlangeDiameterGearSide
            && sameFlangeDiameterNonGearSide
            && sameRimInternalDiameter
            && SpokeCount == other.SpokeCount
            && SpokeLacingPattern == other.SpokeLacingPattern;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(
            Math.Round(HubCenterToFlangeDistanceGearSide.Value, 10),
            Math.Round(HubCenterToFlangeDistanceNonGearSide.Value, 10),
            Math.Round(HubFlangeDiameterGearSide.Value, 10),
            Math.Round(HubFlangeDiameterNonGearSide.Value, 10),
            Math.Round(RimInternalDiameter.Value, 10),
            SpokeCount,
            SpokeLacingPattern);
    }
}

