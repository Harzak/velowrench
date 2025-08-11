using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace velowrench.Calculations.Calculs.Transmission.GearRatio.Development;

public sealed class DevelopmentRatioCalcul : GearRatioCalcul<DevelopmentRatioCalculInput, GearRatioCalculResult>
{
    protected override string CalculName => nameof(DevelopmentRatioCalcul);

    public DevelopmentRatioCalcul(ILogger logger) : base(logger)
    {

    }

    protected override double CalculateRatio(DevelopmentRatioCalculInput input, int teethCountChainring, int teethCountSprocket)
    {
        double gearRatio = base.CalculateGearRatio(teethCountChainring, teethCountSprocket);
        double circumferenceInInch = Math.PI * input.WheelDiameterInInch;
        double developmentInInch = circumferenceInInch * gearRatio;
        return Math.Round(developmentInInch, input.Precision);
    }

    protected override GearRatioCalculResult CreateResult(IList<double> ratiosLargeOrUniqueChainring, 
        IList<double>? ratiosMediumChainring, 
        IList<double>? ratiosSmallChainring, 
        DateTime calculatedAt, 
        DevelopmentRatioCalculInput input)
    {
        return new DevelopmentRatioCalculResult()
        {
            RatiosLargeOrUniqueChainring = ratiosLargeOrUniqueChainring,
            RatiosMediumChainring = ratiosMediumChainring,
            RatiosSmallChainring = ratiosSmallChainring,
            CalculatedAt = calculatedAt,
            UsedInputs = input
        };
    }
}
