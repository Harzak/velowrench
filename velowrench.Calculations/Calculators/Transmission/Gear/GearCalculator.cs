using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using velowrench.Calculations.Exceptions;
using velowrench.Calculations.Interfaces;
using velowrench.Utils.Results;

namespace velowrench.Calculations.Calculators.Transmission.Gear;

/// <summary>
/// Performs gear ratio calculations for bicycle drivetrains.
/// Calculates various gear metrics including gear inches, development, gain ratio, and speed
/// </summary>
public class GearCalculator : BaseCalculator<GearCalculatorInput, GearCalculatorResult>
{
    private const int INNER_CALCULATION_PRECISION = 4;

    protected override string CalculatorName => nameof(GearCalculator);

    public GearCalculator(Func<ICalculatorInputValidation<GearCalculatorInput>> validationProvider, ILogger logger) : base(validationProvider, logger)
    {

    }

    /// <summary>
    /// Performs the gear calculation based on the specified input parameters.
    /// Calculates the appropriate metric (gear inches, development, gain ratio, or speed) 
    /// for each combination of chainrings and sprockets.
    /// </summary>
    protected override OperationResult<GearCalculatorResult> Calculate(GearCalculatorInput input)
    {
        ArgumentNullException.ThrowIfNull(input, nameof(input));

        ICalculatorInputValidation<GearCalculatorInput> validator = base.GetValidation();
        if (!validator.Validate(input))
        {
            throw new CalculatorInputException(validator.ErrorMessages);
        }

        List<double> valuesLargeOrUniqueChainring = [];
        List<double> valuesMediumChainring = [];
        List<double> valuesSmallChainring = [];

        foreach (int teethCount in input.NumberOfTeethBySprocket.Order())
        {
            double ratioLargeChainRing = this.CalculateByMetrics(input, input.TeethNumberLargeOrUniqueChainring, teethCount);
            valuesLargeOrUniqueChainring.Add(ratioLargeChainRing);

            if (input.TeethNumberMediumChainring.HasValue)
            {
                double ratioMediumChainRing = this.CalculateByMetrics(input, input.TeethNumberMediumChainring.Value, teethCount);
                valuesMediumChainring.Add(ratioMediumChainRing);
            }

            if (input.TeethNumberSmallChainring.HasValue)
            {
                double ratioSmallChainRing = this.CalculateByMetrics(input, input.TeethNumberSmallChainring.Value, teethCount);
                valuesSmallChainring.Add(ratioSmallChainRing);
            }
        }

        return new OperationResult<GearCalculatorResult>()
        {
            Content = new GearCalculatorResult()
            {
                CalculatedAt = DateTime.UtcNow,
                ValuesLargeOrUniqueChainring = new ReadOnlyCollection<double>(valuesLargeOrUniqueChainring),
                ValuesMediumChainring = input.TeethNumberMediumChainring.HasValue ? new ReadOnlyCollection<double>(valuesMediumChainring) : null,
                ValuesSmallChainring = input.TeethNumberSmallChainring.HasValue ? new ReadOnlyCollection<double>(valuesSmallChainring) : null,
                UsedInputs = input
            },
            IsSuccess = true
        };
    }

    private double CalculateByMetrics(GearCalculatorInput input, int teethCountChainring, int teethCountSprocket)
    {
        return input.CalculatorType switch
        {
            EGearCalculatorType.Development => this.CalculateDevelopment(input, teethCountChainring, teethCountSprocket, input.Precision),
            EGearCalculatorType.GainRatio => this.CalculateGainRatio(input, teethCountChainring, teethCountSprocket, input.Precision),
            EGearCalculatorType.GearInches => this.CalculateGearInches(input, teethCountChainring, teethCountSprocket, input.Precision),
            EGearCalculatorType.Speed => this.CalculateSpeed(input, teethCountChainring, teethCountSprocket, input.Precision),
            _ => throw new NotSupportedException($"The calculation type '{input.CalculatorType}' is not supported."),
        };
    }

    /// <summary>
    /// Raw ratio between rear sprocket and front chainring used as the basis for other calculations
    /// </summary>
    private double CalculateGearRatio(int teethCountChainring, int teethCountSprocket, int precision = 4)
    {
        return Math.Round(teethCountChainring / (double)teethCountSprocket, precision);
    }

    /// <summary>
    /// Gain ratio calculation that considers crank arm length in the gear ratio formula.
    /// Provides a more accurate representation of mechanical advantage for pedaling efficiency.
    /// </summary>
    private double CalculateGainRatio(GearCalculatorInput input, int teethCountChainring, int teethCountSprocket, int precision)
    {
        double gearRatio = this.CalculateGearRatio(teethCountChainring, teethCountSprocket);
        double gainRatio = ((input.WheelDiameterInInch / 2) / input.CrankLengthInInch!.Value) * gearRatio;
        return Math.Round(gainRatio, precision);
    }

    /// <summary>
    /// Traditional gear inches calculation that represents the diameter of a wheel 
    /// that would travel the same distance per pedal revolution as the actual gear combination.
    /// Standard measurement for comparing gear ratios across different bicycle configurations.
    /// </summary>
    private double CalculateGearInches(GearCalculatorInput input, int teethCountChainring, int teethCountSprocket, int precision)
    {
        double gearRatio = this.CalculateGearRatio(teethCountChainring, teethCountSprocket);
        double gearInches = input.WheelDiameterInInch * gearRatio;
        return Math.Round(gearInches, precision);
    }

    /// <summary>
    /// Development calculation that measures the distance traveled per pedal revolution.
    /// Expressed in inches, this metric directly shows how far the bicycle moves forward 
    /// with each complete pedal stroke for a given gear combination.
    /// </summary>
    private double CalculateDevelopment(GearCalculatorInput input, int teethCountChainring, int teethCountSprocket, int precision)
    {
        double gearRatio = this.CalculateGearRatio(teethCountChainring, teethCountSprocket);
        double circumferenceInInch = Math.PI * input.WheelDiameterInInch;
        double developmentInInch = circumferenceInInch * gearRatio;
        return Math.Round(developmentInInch, precision);
    }

    /// <summary>
    /// Speed calculation that determines theoretical speed based on cadence and gear ratio.
    /// Calculates the speed achievable at a specific pedaling rate (RPM) for each gear combination.
    /// </summary>
    private double CalculateSpeed(GearCalculatorInput input, int teethCountChainring, int teethCountSprocket, int precision)
    {
        double developmentInInch = this.CalculateDevelopment(input, teethCountChainring, teethCountSprocket, INNER_CALCULATION_PRECISION);
        double speedInInchPerMinute = input.RevolutionPerMinute!.Value * developmentInInch;
        return Math.Round(speedInInchPerMinute, precision);
    }
}
