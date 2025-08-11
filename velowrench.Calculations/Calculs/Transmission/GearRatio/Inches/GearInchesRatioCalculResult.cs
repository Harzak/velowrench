using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Calculations.Calculs.Transmission.GearRatio.Gain;

namespace velowrench.Calculations.Calculs.Transmission.GearRatio.Inches;

public sealed record GearInchesRatioCalculResult : GearRatioCalculResult
{
    /// <summary>
    /// Gets the input parameters that were used to produce this calculation result.
    /// </summary>
    public required GearInchesRatioCalculInput UsedInputs { get; init; }
    public GearInchesRatioCalculResult() : base()
    {

    }
}

