namespace velowrench.Calculations.Calculators.Wheels.SpokeLength;

/// <summary>
/// Represents the result of a spoke length calculation.
/// </summary>
public record SpokeLengthCalculatorResult : BaseCalculatorResult<SpokeLengthCalculatorInput>
{
    /// <summary>
    /// Gets the calculated length of the spoke on the non-gear side of the wheel.
    /// </summary>
    public required double SpokeLengthNonGearSideMm { get; init; }

    /// <summary>
    /// Gets the calculated length of the spoke on the gear side of the wheel.
    /// </summary>
    public required double SpokeLengthGearSideMm { get; init; }

    public SpokeLengthCalculatorResult()
    {

    }
}
