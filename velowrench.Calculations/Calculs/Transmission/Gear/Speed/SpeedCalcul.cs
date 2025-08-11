using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Calculations.Calculs.Transmission.Gear.GearInches;

namespace velowrench.Calculations.Calculs.Transmission.Gear.Speed;

public sealed class SpeedCalcul : GearCalcul<SpeedCalculInput, GearCalculResult>
{
    protected override string CalculName => nameof(SpeedCalcul);

    public SpeedCalcul(ILogger logger) : base(logger)
    {

    }

    protected override double CalculateRatio(SpeedCalculInput input, int teethCountChainring, int teethCountSprocket)
    {
        double gearRatio = base.CalculateGearRatio(teethCountChainring, teethCountSprocket);
        double circumferenceInInch = Math.PI * input.WheelDiameterInInch;
        double developmentInInch = circumferenceInInch * gearRatio;
        double speedInInchPerMinute =  input.RotationPerMinute * developmentInInch;
        return Math.Round(speedInInchPerMinute, input.Precision);
    }

    protected override GearCalculResult CreateResult(IList<double> ratiosLargeOrUniqueChainring, 
        IList<double>? ratiosMediumChainring, 
        IList<double>? ratiosSmallChainring, 
        DateTime calculatedAt, 
        SpeedCalculInput input)
    {
        return new SpeedCalculResult()
        {
            RatiosLargeOrUniqueChainring = ratiosLargeOrUniqueChainring,
            RatiosMediumChainring = ratiosMediumChainring,
            RatiosSmallChainring = ratiosSmallChainring,
            CalculatedAt = calculatedAt,
            UsedInputs = input
        };
    }
}
