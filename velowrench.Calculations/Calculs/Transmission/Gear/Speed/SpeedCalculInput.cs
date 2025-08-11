using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace velowrench.Calculations.Calculs.Transmission.Gear.Speed;

public record SpeedCalculInput : GearCalculInput
{
    public required int RotationPerMinute { get; init; }

    public SpeedCalculInput() : base()
    {
    }
}
