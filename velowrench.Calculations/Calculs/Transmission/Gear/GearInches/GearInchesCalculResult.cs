using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Calculations.Calculs.Transmission.Gear.GainRatio;

namespace velowrench.Calculations.Calculs.Transmission.Gear.GearInches;

public sealed record GearInchesCalculResult : GearCalculResult
{
    /// <summary>
    /// Gets the input parameters that were used to produce this calculation result.
    /// </summary>
    public required GearInchesCalculInput UsedInputs { get; init; }
    public GearInchesCalculResult() : base()
    {

    }
}

