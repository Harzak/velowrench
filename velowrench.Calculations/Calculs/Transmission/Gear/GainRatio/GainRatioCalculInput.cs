using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace velowrench.Calculations.Calculs.Transmission.Gear.GainRatio;

public record GainRatioCalculInput : GearCalculInput
{
    public required double CrankLengthInInch { get; init; }

    public GainRatioCalculInput() : base()
    {

    }
}
