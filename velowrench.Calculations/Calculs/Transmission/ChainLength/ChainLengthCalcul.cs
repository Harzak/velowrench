using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Calculations.Exceptions;
using velowrench.Utils.Results;

namespace velowrench.Calculations.Calculs.Transmission.ChainLength;

public sealed class ChainLengthCalcul : BaseCalcul<ChainLengthCalculInput, ChainLengthCalculResult>
{
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

        double C = input.ChainStayLengthInch;
        double F = input.TeethLargestChainring;
        double R = input.TeethLargestSprocket;
        double L = 0;

        L = 2 * C + F /4 + R/4 + 1;

        if (double.IsNaN(L) || double.IsInfinity(L) || L < 0)
        {
            return result.WithError("Calculated chain length is not a valid number.");
        }

        result.Content = new ChainLengthCalculResult
        {
            ChainLinks = (int)Math.Round(L),
            ChainLengthInch = L,
            CalculatedAt = DateTime.UtcNow,
            UsedInputs = input
        };

        return result.WithSuccess();
    }

    //private double CalculateLengthWithBigBig2Equation()
    //{

    //}

    //private double CalculateLengthWithRigorousEquation()
    //{

    //}
}