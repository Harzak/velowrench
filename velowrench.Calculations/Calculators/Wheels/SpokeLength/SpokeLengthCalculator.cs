using Microsoft.Extensions.Logging;
using velowrench.Calculations.Interfaces;
using velowrench.Utils.Results;

namespace velowrench.Calculations.Calculators.Wheels.SpokeLength;

/// <summary>
/// Calculates the lengths of wheel spokes based on input parameters such as rim diameter, hub dimensions, and lacing pattern.
/// </summary>
public sealed class SpokeLengthCalculator : BaseCalculator<SpokeLengthCalculatorInput, SpokeLengthCalculatorResult>
{
    /// <summary>
    /// Standard size of a spoke hole in millimeters.
    /// </summary>
    private const double SPOKE_HOLE_DIAMETER_MM = 2.5;

    protected override string CalculatorName => nameof(SpokeLengthCalculator);

    public override ICalculatorInputValidator<SpokeLengthCalculatorInput> InputValidator { get; }

    public SpokeLengthCalculator(ICalculatorInputValidator<SpokeLengthCalculatorInput> inputValidator, ILogger logger) : base(logger)
    {
        this.InputValidator = inputValidator ?? throw new ArgumentNullException(nameof(inputValidator));
    }

    protected override OperationResult<SpokeLengthCalculatorResult> Calculate(SpokeLengthCalculatorInput input)
    {
        double spokeLengthNonGearSideMm = this.GetCorrectedSpokeLength(
            input.RimInternalDiameterMm,
            input.HubCenterToFlangeDistanceNonGearSideMm,
            input.HubFlangeDiameterNonGearSideMm,
            input.SpokeLacingPattern,
            input.SpokeCount);

        double spokeLengthGearSideMm = this.GetCorrectedSpokeLength(
            input.RimInternalDiameterMm,
            input.HubCenterToFlangeDistanceGearSideMm,
            input.HubFlangeDiameterGearSideMm,
            input.SpokeLacingPattern,
            input.SpokeCount);

        return new OperationResult<SpokeLengthCalculatorResult>()
        {
            Content = new SpokeLengthCalculatorResult()
            {
                SpokeLengthNonGearSideMm = Math.Round(spokeLengthNonGearSideMm, input.Precision),
                SpokeLengthGearSideMm = Math.Round(spokeLengthGearSideMm, input.Precision),
                CalculatedAt = DateTime.UtcNow,
                UsedInputs = input.Copy(), //since we use mutable type for input, copy is necessary to avoid external mutations
            },
            IsSuccess = true
        };
    }

    /// <summary>
    /// Calculates the corrected spoke length by accounting for the spoke hole radius.
    /// </summary>
    private double GetCorrectedSpokeLength(double rimInternalDiameterMm, double hubCenterToFalngeDistanceMm, double hubFlangeDiameter, double spokeLacingPattern, int spokeCount)
    {
        double length = this.GetSpokeLength(rimInternalDiameterMm, hubCenterToFalngeDistanceMm, hubFlangeDiameter, spokeLacingPattern, spokeCount);
        return length - (SPOKE_HOLE_DIAMETER_MM / 2.0);
    }

    /// <summary>
    /// Calculates the length of a spoke in a 3D space based on the chord length: hypotenuse including flange offset
    /// </summary>
    private double GetSpokeLength(double rimInternalDiameterMm, double hubCenterToFlangeDistanceMm, double hubFlangeDiameter, double spokeLacingPattern, int spokeCount)
    {
        double chord = this.GetChordLength(rimInternalDiameterMm, hubFlangeDiameter, spokeLacingPattern, spokeCount);
        return Math.Sqrt(chord * chord + hubCenterToFlangeDistanceMm * hubCenterToFlangeDistanceMm);
    }

    /// <summary>
    /// Law of cosines for the chord distance in the rim plane
    /// </summary>
    private double GetChordLength(double rimInternalDiameterMm, double hubFlangeDiameter, double spokeLacingPattern, int spokeCount)
    {
        double rimRadius = rimInternalDiameterMm / 2.0;
        double flangeRadius = hubFlangeDiameter / 2.0;
        double theta = this.GetSpokeAndRimHoleAngle(spokeLacingPattern, spokeCount);
        return Math.Sqrt(rimRadius * rimRadius + flangeRadius * flangeRadius - 2.0 * rimRadius * flangeRadius * Math.Cos(theta));
    }

    /// <summary>
    /// Calculates the angle between a spoke and its corresponding rim hole.
    /// </summary>
    private double GetSpokeAndRimHoleAngle(double spokeLacingPattern, int spokeCount)
    {
        return 2.0 * Math.PI * spokeLacingPattern / (spokeCount / 2);
    }
}
