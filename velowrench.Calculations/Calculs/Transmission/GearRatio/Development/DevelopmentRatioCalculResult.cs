using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace velowrench.Calculations.Calculs.Transmission.GearRatio.Development;

public sealed record DevelopmentRatioCalculResult : GearRatioCalculResult
{
    /// <summary>
    /// Gets the input parameters that were used to produce this calculation result.
    /// </summary>
    public required DevelopmentRatioCalculInput UsedInputs { get; init; }

    public DevelopmentRatioCalculResult(): base()
    {
    }
}
