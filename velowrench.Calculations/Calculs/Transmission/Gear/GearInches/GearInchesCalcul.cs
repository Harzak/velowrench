using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace velowrench.Calculations.Calculs.Transmission.Gear.GearInches;

public sealed class GearInchesCalcul : GearCalcul<GearInchesCalculInput, GearCalculResult>
{
    protected override string CalculName => nameof(GearInchesCalcul);

    public GearInchesCalcul(ILogger logger) : base(logger)
    {

    }

    protected override double CalculateRatio(GearInchesCalculInput input, int teethCountChainring, int teethCountSprocket)
    {
        double gearRatio = base.CalculateGearRatio(teethCountChainring, teethCountSprocket);
        double gearInches = input.WheelDiameterInInch * gearRatio;
        return Math.Round(gearInches, input.Precision);
    }

    protected override GearCalculResult CreateResult(IList<double> ratiosLargeOrUniqueChainring, 
        IList<double>? ratiosMediumChainring, 
        IList<double>? ratiosSmallChainring,
        DateTime calculatedAt, 
        GearInchesCalculInput input)
    {
        return new GearInchesCalculResult()
        {
            RatiosLargeOrUniqueChainring = ratiosLargeOrUniqueChainring,
            RatiosMediumChainring = ratiosMediumChainring,
            RatiosSmallChainring = ratiosSmallChainring,
            CalculatedAt = calculatedAt,
            UsedInputs = input
        };
    }
}
