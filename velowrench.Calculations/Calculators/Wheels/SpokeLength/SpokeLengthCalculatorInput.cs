using velowrench.Calculations.Constants;

namespace velowrench.Calculations.Calculators.Wheels.SpokeLength;

/// <summary>
/// Represents the input parameters required for calculating spoke lengths in a wheel-building context.
/// </summary>
public sealed record SpokeLengthCalculatorInput
{
    /// <summary>
    /// Gets the distance from the hub center to the flange on the gear (right) side.
    /// </summary>
    public required double HubCenterToFlangeDistanceGearSideMm { get; init; }

    /// <summary>
    /// Gets the distance from the hub center to the flange on the non-gear (left) side.
    /// </summary>
    public required double HubCenterToFlangeDistanceNonGearSideMm { get; init; }

    /// <summary>
    /// Gets the diameter of the circle through the centers of the spoke holes on the gear side (right) flange.
    /// </summary>
    public required double HubFlangeDiameterGearSideMm { get; init; }

    /// <summary>
    /// Gets the diameter of the circle through the centers of the spoke holes on the non-gear side (left) flange.
    /// </summary>
    public required double HubFlangeDiameterNonGearSideMm { get; init; }

    /// <summary>
    /// Gets the internal diameter / effective Rim Diameter (ERD) of the rim. The diameter measured at the nipple seats inside the rim
    /// </summary>
    public required double RimInternalDiameterMm { get; init; }

    /// <summary>
    /// Gets the number of spokes in the wheel (hole count).
    /// </summary>
    public required int SpokeCount { get; init; }

    /// <summary>
    /// Gets the spoke lacing pattern (cross count). How many times each spoke crosses others (e.g., 3-cross, 2-cross, radial).
    /// </summary>
    public required int SpokeLacingPattern { get; init; }

    /// <summary>
    /// Gets the number of decimal places to include in calculation results.
    /// Controls the precision of the output values.
    /// </summary>
    public int Precision { get; init; }

    public SpokeLengthCalculatorInput()
    {
        this.Precision = CalculationConstants.DEFAULT_PRECISION;
    }

    /// <summary>
    /// Custom equality implementation with floating-point tolerance.
    /// </summary>
    public bool Equals(SpokeLengthCalculatorInput? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;

        const double tolerance = 1e-10;

        bool sameCenterToFlangeDistanceGearSide = Math.Abs(HubCenterToFlangeDistanceGearSideMm - other.HubCenterToFlangeDistanceGearSideMm) < tolerance;
        bool sameCenterToFlangeDistanceNonGearSide = Math.Abs(HubCenterToFlangeDistanceNonGearSideMm - other.HubCenterToFlangeDistanceNonGearSideMm) < tolerance;
        bool sameFlangeDiameterGearSide = Math.Abs(HubFlangeDiameterGearSideMm - other.HubFlangeDiameterGearSideMm) < tolerance;
        bool sameFlangeDiameterNonGearSide = Math.Abs(HubFlangeDiameterNonGearSideMm - other.HubFlangeDiameterNonGearSideMm) < tolerance;
        bool sameRimInternalDiameter = Math.Abs(RimInternalDiameterMm - other.RimInternalDiameterMm) < tolerance;
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
            Math.Round(HubCenterToFlangeDistanceGearSideMm, 10),
            Math.Round(HubCenterToFlangeDistanceNonGearSideMm, 10),
            Math.Round(HubFlangeDiameterGearSideMm, 10),
            Math.Round(HubFlangeDiameterNonGearSideMm, 10),
            Math.Round(RimInternalDiameterMm, 10),
            SpokeCount,
            SpokeLacingPattern);
    }
}

