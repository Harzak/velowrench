using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace velowrench.Calculations.Calculs.Transmission.Gear.Development;

public sealed class DevelopmentCalcul : GearCalcul<DevelopmentCalculInput, GearCalculResult>
{
    protected override string CalculName => nameof(DevelopmentCalcul);

    public DevelopmentCalcul(ILogger logger) : base(logger)
    {

    }

    protected override double CalculateRatio(DevelopmentCalculInput input, int teethCountChainring, int teethCountSprocket)
    {
        double gearRatio = base.CalculateGearRatio(teethCountChainring, teethCountSprocket);
        double circumferenceInInch = Math.PI * input.WheelDiameterInInch;
        double developmentInInch = circumferenceInInch * gearRatio;
        return Math.Round(developmentInInch, input.Precision);
    }

    protected override GearCalculResult CreateResult(IList<double> ratiosLargeOrUniqueChainring, 
        IList<double>? ratiosMediumChainring, 
        IList<double>? ratiosSmallChainring, 
        DateTime calculatedAt, 
        DevelopmentCalculInput input)
    {
        return new DevelopmentCalculResult()
        {
            RatiosLargeOrUniqueChainring = ratiosLargeOrUniqueChainring,
            RatiosMediumChainring = ratiosMediumChainring,
            RatiosSmallChainring = ratiosSmallChainring,
            CalculatedAt = calculatedAt,
            UsedInputs = input
        };
    }
}
