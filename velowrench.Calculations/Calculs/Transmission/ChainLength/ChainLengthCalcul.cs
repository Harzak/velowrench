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
        CalculInputException.ThrowIfNegativeOrZero(input.ChainStayLength, nameof(input.ChainStayLength));
        CalculInputException.ThrowIfNegativeOrZero(input.LargestSprocket, nameof(input.LargestSprocket));
        CalculInputException.ThrowIfNegativeOrZero(input.LargestChainring, nameof(input.LargestChainring));

        OperationResult<ChainLengthCalculResult> result = new();

        double C = input.ChainStayLength;
        double F = input.LargestChainring;
        double R = input.LargestSprocket;
        double L = 0;

        L = 2 * C + F /4 + R/4 + 1;

        if (double.IsNaN(L) || double.IsInfinity(L) || L < 0)
        {
            return result.WithError("Calculated chain length is not a valid number.");
        }

        result.Content = new ChainLengthCalculResult
        {
            ChainLengthInLinks = (int)Math.Round(L),
            ChainLengthInInches = L,
            ChainLengthInMm = L * 25.4,
            CalculatedAt = DateTime.UtcNow,
            UsedInput = input
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