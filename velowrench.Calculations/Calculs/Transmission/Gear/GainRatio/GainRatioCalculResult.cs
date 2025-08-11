using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace velowrench.Calculations.Calculs.Transmission.Gear.GainRatio;

public sealed record class GainRatioCalculResult : GearCalculResult
{
    /// <summary>
    /// Gets the input parameters that were used to produce this calculation result.
    /// </summary>
    public required GainRatioCalculInput UsedInputs { get; init; }
    public GainRatioCalculResult() : base()
    {

    }
}
