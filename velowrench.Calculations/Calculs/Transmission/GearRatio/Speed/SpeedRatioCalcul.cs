using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Calculations.Calculs.Transmission.GearRatio.Inches;

namespace velowrench.Calculations.Calculs.Transmission.GearRatio.Speed;

public sealed class SpeedRatioCalcul : GearRatioCalcul<SpeedRatioCalculInput, GearRatioCalculResult>
{
    protected override string CalculName => nameof(SpeedRatioCalcul);

    public SpeedRatioCalcul(ILogger logger) : base(logger)
    {

    }

    protected override double CalculateRatio(SpeedRatioCalculInput input, int teethCountChainring, int teethCountSprocket)
    {
        double gearRatio = base.CalculateGearRatio(teethCountChainring, teethCountSprocket);
        double circumferenceInInch = Math.PI * input.WheelDiameterInInch;
        double developmentInInch = circumferenceInInch * gearRatio;
        double speedInInchPerMinute =  input.RotationPerMinute * developmentInInch;
        return Math.Round(speedInInchPerMinute, input.Precision);
    }

    protected override GearRatioCalculResult CreateResult(IList<double> ratiosLargeOrUniqueChainring, 
        IList<double>? ratiosMediumChainring, 
        IList<double>? ratiosSmallChainring, 
        DateTime calculatedAt, 
        SpeedRatioCalculInput input)
    {
        return new SpeedRatioCalculResult()
        {
            RatiosLargeOrUniqueChainring = ratiosLargeOrUniqueChainring,
            RatiosMediumChainring = ratiosMediumChainring,
            RatiosSmallChainring = ratiosSmallChainring,
            CalculatedAt = calculatedAt,
            UsedInputs = input
        };
    }
}
