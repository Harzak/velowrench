using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Calculations.Calculs.Transmission.ChainLength;
using velowrench.Calculations.Exceptions;
using velowrench.Utils.Results;

namespace velowrench.Calculations.Calculs.Transmission.GearRatio;

public abstract class GearRatioCalcul<TInput, TResult> :  BaseCalcul<TInput, TResult>  where TInput : GearRatioCalculInput where TResult : GearRatioCalculResult
{
    protected GearRatioCalcul(ILogger logger) : base(logger)
    {

    }

    protected override OperationResult<TResult> Calculate(TInput input) 
    {
        ArgumentNullException.ThrowIfNull(input, nameof(input));
        CalculInputException.ThrowIfNegativeOrZero(input.TeethNumberLargeOrUniqueChainring, nameof(input.TeethNumberLargeOrUniqueChainring));
        CalculInputException.ThrowIfNegative(input.Precision, nameof(input.TeethNumberLargeOrUniqueChainring));

        if (input.TeethNumberMediumChainring.HasValue)
        {
            CalculInputException.ThrowIfNegativeOrZero(input.TeethNumberMediumChainring.Value, nameof(input.TeethNumberMediumChainring));
        }
        if (input.TeethNumberSmallChainring.HasValue)
        {
            CalculInputException.ThrowIfNegativeOrZero(input.TeethNumberSmallChainring.Value, nameof(input.TeethNumberSmallChainring));
        }

        List<double> ratiosLargeOrUniqueChainring = [];
        List<double> ratiosMediumChainring = [];
        List<double> ratiosSmallChainring = [];

        foreach (int teethCount in input.NumberOfTeethBySprocket.Order())
        {
            double ratioLargeChainRing = this.CalculateRatio(input, input.TeethNumberLargeOrUniqueChainring, teethCount);
            ratiosLargeOrUniqueChainring.Add(ratioLargeChainRing);

            if (input.TeethNumberMediumChainring.HasValue)
            {
                double ratioMediumChainRing = this.CalculateRatio(input, input.TeethNumberMediumChainring.Value, teethCount);
                ratiosMediumChainring.Add(ratioMediumChainRing);
            }

            if (input.TeethNumberSmallChainring.HasValue)
            {
                double ratioSmallChainRing = this.CalculateRatio(input, input.TeethNumberSmallChainring.Value, teethCount);
                ratiosSmallChainring.Add(ratioSmallChainRing);
            }
        }

        OperationResult<TResult> result = new()
        {
            Content = this.CreateResult(ratiosLargeOrUniqueChainring,
                input.TeethNumberMediumChainring.HasValue ? ratiosMediumChainring : null,
                input.TeethNumberSmallChainring.HasValue ? ratiosSmallChainring : null,
                DateTime.UtcNow,
                input)
        };

        return result.WithSuccess();
    }

    protected abstract TResult CreateResult(IList<double> ratiosLargeOrUniqueChainring,
        IList<double>? ratiosMediumChainring,
        IList<double>? ratiosSmallChainring,
        DateTime calculatedAt,
        TInput input);

    protected abstract double CalculateRatio(TInput input, int teethCountChainring, int teethCountSprocket);

    protected double CalculateGearRatio(int teethCountChainring, int teethCountSprocket, int precision = 4)
    {
        return Math.Round(teethCountChainring / (double)teethCountSprocket, precision);
    }
}
