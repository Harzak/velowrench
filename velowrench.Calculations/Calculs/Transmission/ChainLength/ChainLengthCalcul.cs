using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Calculations.Constants;
using velowrench.Calculations.Exceptions;
using velowrench.Utils.Results;

namespace velowrench.Calculations.Calculs.Transmission.ChainLength;

public sealed class ChainLengthCalcul : BaseCalcul<ChainLengthCalculInput, ChainLengthCalculResult>
{
    /// <summary>
    /// A conversion factor from tooth count difference to chain angle correction in inches
    /// - to approximate chain slack/tension effect due to gear size mismatch.
    /// </summary>
    private const double ANGULAR_OFFSET_FACTOR = 0.0796;

    /// <summary>
    /// BigBig2 equation lose accuracy for chainstays smaller than this value.
    /// </summary>
    private const double CHAINSTAY_TRESHOLD = 15.5;

    protected override string CalculName => nameof(ChainLengthCalcul);

    public ChainLengthCalcul(ILogger logger) : base(logger)
    {

    }

    protected override OperationResult<ChainLengthCalculResult> Calculate(ChainLengthCalculInput input)
    {
        ArgumentNullException.ThrowIfNull(input, nameof(input));
        CalculInputException.ThrowIfNegativeOrZero(input.ChainStayLengthInch, nameof(input.ChainStayLengthInch));
        CalculInputException.ThrowIfNegativeOrZero(input.TeethLargestSprocket, nameof(input.TeethLargestSprocket));
        CalculInputException.ThrowIfNegativeOrZero(input.TeethLargestChainring, nameof(input.TeethLargestChainring));

        OperationResult<ChainLengthCalculResult> result = new();

        double calculatedLength;
        if (input.ChainStayLengthInch < CHAINSTAY_TRESHOLD)
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

        result.Content = new ChainLengthCalculResult
        {
            ChainLinks = this.GetChainLinksNumber(calculatedLength),
            ChainLengthInch = calculatedLength,
            CalculatedAt = DateTime.UtcNow,
            UsedInputs = input
        };

        return result.WithSuccess();
    }

    /// <summary>
    /// L = 2 * C + F /4 + R/4 + 1;
    /// </summary>
    private double CalculateLengthWithBigBig2Equation(ChainLengthCalculInput input)
    {
        double C = input.ChainStayLengthInch;
        double F = input.TeethLargestChainring;
        double R = input.TeethLargestSprocket;

        double doubleC = ChainConst.CHAINSTAY_MULTIPLIER * C;
        double averageFR = ChainConst.TEETH_WRAP_RATIO * (F + R);
        double offset = 1;

        double L = doubleC + averageFR + offset;

        return L;
    }

    /// <summary>
    /// L = 0.25 * (F + R) + 2 * Math.Sqrt(Math.Pow(C, 2) + Math.Pow(0.0796 * (F - R), 2));
    /// </summary>
    private double CalculateLengthWithRigorousEquation(ChainLengthCalculInput input)
    {
        double C = input.ChainStayLengthInch;
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