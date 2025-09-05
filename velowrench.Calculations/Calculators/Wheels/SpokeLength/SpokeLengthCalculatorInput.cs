using velowrench.Calculations.Constants;

namespace velowrench.Calculations.Calculators.Wheels.SpokeLength;

/// <summary>
/// Represents the input parameters required for calculating spoke lengths in a wheel-building context.
/// </summary>
public sealed class SpokeLengthCalculatorInput : BaseCalculatorInput, IEquatable<SpokeLengthCalculatorInput>
{
    /// <summary>
    /// Gets the distance from the hub center to the flange on the gear (right) side.
    /// </summary>
    public double HubCenterToFlangeDistanceGearSideMm { get; set; }

    /// <summary>
    /// Gets the distance from the hub center to the flange on the non-gear (left) side.
    /// </summary>
    public double HubCenterToFlangeDistanceNonGearSideMm { get; set; }

    /// <summary>
    /// Gets the diameter of the circle through the centers of the spoke holes on the gear side (right) flange.
    /// </summary>
    public double HubFlangeDiameterGearSideMm { get; set; }

    /// <summary>
    /// Gets the diameter of the circle through the centers of the spoke holes on the non-gear side (left) flange.
    /// </summary>
    public double HubFlangeDiameterNonGearSideMm { get; set; }

    /// <summary>
    /// Gets the internal diameter / effective Rim Diameter (ERD) of the rim. The diameter measured at the nipple seats inside the rim
    /// </summary>
    public double RimInternalDiameterMm { get; set; }

    /// <summary>
    /// Gets the number of spokes in the wheel (hole count).
    /// </summary>
    public int SpokeCount { get; set; }

    /// <summary>
    /// Gets the spoke lacing pattern (cross count). How many times each spoke crosses others (e.g., 3-cross, 2-cross, radial).
    /// </summary>
    public int SpokeLacingPattern { get; set; }

    public SpokeLengthCalculatorInput() : base()
    {

    }

    public SpokeLengthCalculatorInput(int precision) : base(precision)
    {

    }

    internal SpokeLengthCalculatorInput Copy()
    {
        return new SpokeLengthCalculatorInput(base.Precision)
        {
            HubCenterToFlangeDistanceGearSideMm = this.HubCenterToFlangeDistanceGearSideMm,
            HubCenterToFlangeDistanceNonGearSideMm = this.HubCenterToFlangeDistanceNonGearSideMm,
            HubFlangeDiameterGearSideMm = this.HubFlangeDiameterGearSideMm,
            HubFlangeDiameterNonGearSideMm = this.HubFlangeDiameterNonGearSideMm,
            RimInternalDiameterMm = this.RimInternalDiameterMm,
            SpokeCount = this.SpokeCount,
            SpokeLacingPattern = this.SpokeLacingPattern
        };
    }

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

    public override bool Equals(object? obj) => Equals(obj as SpokeLengthCalculatorInput);

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

