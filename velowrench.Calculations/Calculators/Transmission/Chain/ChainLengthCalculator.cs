using Microsoft.Extensions.Logging;
using velowrench.Calculations.Constants;
using velowrench.Calculations.Exceptions;
using velowrench.Calculations.Interfaces;
using velowrench.Utils.Results;

namespace velowrench.Calculations.Calculators.Transmission.Chain;

/// <summary>
/// Calculates the recommended bicycle chain length using bike’s chainstay length and gear sizes.
/// </summary>
public sealed class ChainLengthCalculator : BaseCalculator<ChainLengthCalculatorInput, ChainLengthCalculatorResult>
{
    /// <summary>
    /// A conversion factor from tooth count difference to chain angle correction in inches
    /// - to approximate chain slack/tension effect due to gear size mismatch.
    /// </summary>
    private const double ANGULAR_OFFSET_FACTOR = 0.0796;

    /// <summary>
    /// BigBig2 equation lose accuracy for chainstays smaller than this value.
    /// </summary>
    private const double CHAINSTAY_INCH_THRESHOLD = 15.5;

    protected override string CalculatorName => nameof(ChainLengthCalculator);

    public ChainLengthCalculator(Func<ICalculatorInputValidation<ChainLengthCalculatorInput>> validationProvider, ILogger logger) : base(validationProvider, logger)
    {

    }

    protected override OperationResult<ChainLengthCalculatorResult> Calculate(ChainLengthCalculatorInput input)
    {
        ArgumentNullException.ThrowIfNull(input, nameof(input));

        ICalculatorInputValidation<ChainLengthCalculatorInput> validator = base.GetValidation();
        if (!validator.Validate(input))
        {
            throw new CalculatorInputException(validator.ErrorMessages);
        }


        OperationResult<ChainLengthCalculatorResult> result = new();

        double calculatedLength;
        if (input.ChainStayLengthIn < CHAINSTAY_INCH_THRESHOLD)
        {
            calculatedLength = this.CalculateLengthWithRigorousEquation(input);
        }
        else
        {
            calculatedLength = this.CalculateLengthWithBigBig2Equation(input);
        }

        if (double.IsNaN(calculatedLength) || double.IsInfinity(calculatedLength) || calculatedLength < 0)
        {
            return result.WithError("Calculated chain length is not a valid number.");
        }

        result.Content = new ChainLengthCalculatorResult()
        {
            ChainLinks = this.GetChainLinksNumber(calculatedLength),
            ChainLengthIn = calculatedLength,
            CalculatedAt = DateTime.UtcNow,
            UsedInputs = input.Copy() //since we use mutable type for input, copy is necessary to avoid external mutations
        };

        return result.WithSuccess();
    }

    /// <summary>
    /// The BigBig2 equation: L = 2 * C + F/4 + R/4 + 1, used for longer chainstays.
    /// </summary>
    /// <remarks>
    /// This equation provides a quick approximation suitable for most standard bicycle configurations
    /// where the chainstay length is greater than 15.5 inches.
    /// </remarks>
    private double CalculateLengthWithBigBig2Equation(ChainLengthCalculatorInput input)
    {
        double C = input.ChainStayLengthIn;
        double F = input.TeethLargestChainring;
        double R = input.TeethLargestSprocket;

        double doubleC = ChainConst.CHAINSTAY_MULTIPLIER * C;
        double averageFR = ChainConst.TEETH_WRAP_RATIO * (F + R);
        double offset = 1;

        double L = doubleC + averageFR + offset;

        return L;
    }

    /// <summary>
    /// The precise equation: L = 0.25 * (F + R) + 2 * √(C² + (0.0796 * (F - R))²), used for shorter chainstays.
    /// </summary>
    /// <remarks>
    /// This equation accounts for angular effects and provides higher accuracy for compact bicycle frames
    /// with shorter chainstays (typically less than 15.5 inches).
    /// </remarks>
    private double CalculateLengthWithRigorousEquation(ChainLengthCalculatorInput input)
    {
        double C = input.ChainStayLengthIn;
        double F = input.TeethLargestChainring;
        double R = input.TeethLargestSprocket;

        double delta = F - R;
        double average = ChainConst.TEETH_WRAP_RATIO * (F + R);
        double distance = ChainConst.CHAINSTAY_MULTIPLIER * Math.Sqrt(C * C + Math.Pow(ANGULAR_OFFSET_FACTOR * delta, 2));
        double L = average + distance;

        return L;
    }

    /// <summary>
    /// Get the number of chain links needed for the given chain length in inches.
    /// Round up to have enough chain length (better to be slightly longer than too short).
    /// </summary>
    private int GetChainLinksNumber(double chainLengthInch)
    {
        return (int)Math.Ceiling(chainLengthInch / ChainConst.CHAINLINK_LENGTH_INCH);
    }
}