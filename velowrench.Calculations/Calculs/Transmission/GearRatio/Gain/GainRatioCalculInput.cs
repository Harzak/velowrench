using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace velowrench.Calculations.Calculs.Transmission.GearRatio.Gain;

public record GainRatioCalculInput : GearRatioCalculInput
{
    public required double CrankLengthInInch { get; init; }

    public GainRatioCalculInput() : base()
    {

    }
}
