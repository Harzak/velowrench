using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Calculations.Calculs.Transmission.ChainLength;

namespace velowrench.Calculations.Calculs.Transmission.GearRatio;

/// <summary>
/// Represents the result of a gear ratio calculations.
/// </summary>
public record GearRatioCalculResult
{


    /// <summary>
    /// Gets the UTC timestamp when this calculation was performed.
    /// </summary>
    public required DateTime CalculatedAt { get; init; }

    /// <summary>
    /// Gets the input parameters that were used to produce this calculation result.
    /// </summary>
    public required ChainLengthCalculInput UsedInputs { get; init; }
}
