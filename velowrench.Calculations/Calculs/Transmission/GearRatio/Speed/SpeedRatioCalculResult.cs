using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Calculations.Calculs.Transmission.GearRatio.Inches;

namespace velowrench.Calculations.Calculs.Transmission.GearRatio.Speed;

public sealed record SpeedRatioCalculResult : GearRatioCalculResult
{
    /// <summary>
    /// Gets the input parameters that were used to produce this calculation result.
    /// </summary>
    public required SpeedRatioCalculInput UsedInputs { get; init; }
    public SpeedRatioCalculResult() : base()
    {

    }
}
