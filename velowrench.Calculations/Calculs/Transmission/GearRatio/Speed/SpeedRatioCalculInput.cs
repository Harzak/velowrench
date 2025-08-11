using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace velowrench.Calculations.Calculs.Transmission.GearRatio.Speed;

public record SpeedRatioCalculInput : GearRatioCalculInput
{
    public required int RotationPerMinute { get; init; }

    public SpeedRatioCalculInput() : base()
    {
    }
}
