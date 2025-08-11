using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Calculations.Calculs.Transmission.Gear.GearInches;

namespace velowrench.Calculations.Calculs.Transmission.Gear.Speed;

public sealed record SpeedCalculResult : GearCalculResult
{
    /// <summary>
    /// Gets the input parameters that were used to produce this calculation result.
    /// </summary>
    public required SpeedCalculInput UsedInputs { get; init; }
    public SpeedCalculResult() : base()
    {

    }
}
