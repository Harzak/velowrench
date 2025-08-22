using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitsNet;
using UnitsNet.Units;
using velowrench.Calculations.Calculators.Transmission.Chain;
using velowrench.Calculations.Exceptions;
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

    public SpokeLengthCalculator(Func<ICalculatorInputValidation<SpokeLengthCalculatorInput>> validationProvider, ILogger logger) : base(validationProvider, logger)
    {

    }

    protected override OperationResult<SpokeLengthCalculatorResult> Calculate(SpokeLengthCalculatorInput input)
    {
        ArgumentNullException.ThrowIfNull(input, nameof(input));

        ICalculatorInputValidation<SpokeLengthCalculatorInput> validator = base.GetValidation();
        if (!validator.Validate(input))
        {
            throw new CalculatorInputException(validator.ErrorMessages);
        }

        double spokeLengthNonGearSideMm = this.GetCorrectedSpokeLength(
            input.RimInternalDiameter.GetValueIn(LengthUnit.Millimeter),
            input.HubCenterToFlangeNonGearSideDistance.GetValueIn(LengthUnit.Millimeter),
            input.HubFlangeDiameterNonGearSide.GetValueIn(LengthUnit.Millimeter),
            input.SpokeLacingPattern,
            input.SpokeCount);

        double spokeLengthGearSideMm = this.GetCorrectedSpokeLength(
            input.RimInternalDiameter.GetValueIn(LengthUnit.Millimeter),
            input.HubCenterToFlangeGearSideDistance.GetValueIn(LengthUnit.Millimeter),
            input.HubFlangeDiameterGearSide.GetValueIn(LengthUnit.Millimeter),
            input.SpokeLacingPattern,
            input.SpokeCount);

        return new OperationResult<SpokeLengthCalculatorResult>()
        {
            Content = new SpokeLengthCalculatorResult()
            {
                SpokeLengthNonGearSide = new Units.ConvertibleDouble<LengthUnit>(Math.Round(spokeLengthNonGearSideMm), LengthUnit.Millimeter),
                SpokeLengthGearSide = new Units.ConvertibleDouble<LengthUnit>(Math.Round(spokeLengthGearSideMm), LengthUnit.Millimeter),
                CalculatedAt = DateTime.UtcNow,
                UsedInputs = input,
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
    private double GetSpokeLength(double rimInternalDiameterMm, double hubCenterToFalngeDistanceMm, double hubFlangeDiameter, double spokeLacingPattern, int spokeCount)
    {
        double chord = this.GetChordLength(rimInternalDiameterMm, hubFlangeDiameter, spokeLacingPattern, spokeCount);
        return Math.Sqrt(chord * chord + hubCenterToFalngeDistanceMm * hubCenterToFalngeDistanceMm);
    }

    /// <summary>
    /// Law of cosines for the chord distance in the rim plane
    /// </summary>
    private double GetChordLength(double rimInternalDiameterMm, double hubFlangeDiameter, double spokeLacingPattern, int spokeCount)
    {
        double rimRadius = this.GetRimRadius(rimInternalDiameterMm);
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

    /// <summary>
    /// Calculates the radius of a rim based on its internal diameter.
    /// </summary>
    private double GetRimRadius(double rimInternalDiameterMm)
    {
        return rimInternalDiameterMm / 2.0;
    }
}
