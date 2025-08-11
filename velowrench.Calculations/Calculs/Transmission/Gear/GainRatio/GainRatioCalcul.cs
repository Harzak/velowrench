using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace velowrench.Calculations.Calculs.Transmission.Gear.GainRatio;

public sealed class GainRatioCalcul : GearCalcul<GainRatioCalculInput, GearCalculResult>
{
    protected override string CalculName => nameof(GainRatioCalcul);

    public GainRatioCalcul(ILogger logger) : base(logger)
    {

    }

    protected override double CalculateRatio(GainRatioCalculInput input, int teethCountChainring, int teethCountSprocket)
    {
        double gearRatio = base.CalculateGearRatio(teethCountChainring, teethCountSprocket);
        double gainRatio = ((input.WheelDiameterInInch / 2) / input.CrankLengthInInch) * gearRatio;
        return Math.Round(gainRatio, input.Precision);
    }

    protected override GearCalculResult CreateResult(IList<double> ratiosLargeOrUniqueChainring,
        IList<double>? ratiosMediumChainring,
        IList<double>? ratiosSmallChainring,
        DateTime calculatedAt,
        GainRatioCalculInput input)
    {
        return new GainRatioCalculResult()
        {
            RatiosLargeOrUniqueChainring = ratiosLargeOrUniqueChainring,
            RatiosMediumChainring = ratiosMediumChainring,
            RatiosSmallChainring = ratiosSmallChainring,
            CalculatedAt = calculatedAt,
            UsedInputs = input
        };
    }
}
