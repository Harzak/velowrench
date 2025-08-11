using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace velowrench.Calculations.Calculs.Transmission.GearRatio.Gain;

public sealed record class GainRatioCalculResult : GearRatioCalculResult
{
    /// <summary>
    /// Gets the input parameters that were used to produce this calculation result.
    /// </summary>
    public required GainRatioCalculInput UsedInputs { get; init; }
    public GainRatioCalculResult() : base()
    {

    }
}
