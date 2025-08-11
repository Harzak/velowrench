using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace velowrench.Calculations.Calculs.Transmission.GearRatio.Inches;

public sealed class GearInchesRatioCalcul : GearRatioCalcul<GearInchesRatioCalculInput, GearRatioCalculResult>
{
    protected override string CalculName => nameof(GearInchesRatioCalcul);

    public GearInchesRatioCalcul(ILogger logger) : base(logger)
    {

    }

    protected override double CalculateRatio(GearInchesRatioCalculInput input, int teethCountChainring, int teethCountSprocket)
    {
        double gearRatio = base.CalculateGearRatio(teethCountChainring, teethCountSprocket);
        double gearInches = input.WheelDiameterInInch * gearRatio;
        return Math.Round(gearInches, input.Precision);
    }

    protected override GearRatioCalculResult CreateResult(IList<double> ratiosLargeOrUniqueChainring, 
        IList<double>? ratiosMediumChainring, 
        IList<double>? ratiosSmallChainring,
        DateTime calculatedAt, 
        GearInchesRatioCalculInput input)
    {
        return new GearInchesRatioCalculResult()
        {
            RatiosLargeOrUniqueChainring = ratiosLargeOrUniqueChainring,
            RatiosMediumChainring = ratiosMediumChainring,
            RatiosSmallChainring = ratiosSmallChainring,
            CalculatedAt = calculatedAt,
            UsedInputs = input
        };
    }
}
