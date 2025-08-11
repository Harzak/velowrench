using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Calculations.Calculs.Transmission.Chain;

namespace velowrench.Calculations.Calculs.Transmission.Gear;

/// <summary>
/// Represents the result of a gear ratio calculations.
/// </summary>
public abstract record GearCalculResult
{
    public required IList<double> RatiosLargeOrUniqueChainring { get; init; }
    public IList<double>? RatiosMediumChainring { get; init; }
    public IList<double>? RatiosSmallChainring { get; init; }

    /// <summary>
    /// Gets the UTC timestamp when this calculation was performed.
    /// </summary>
    public required DateTime CalculatedAt { get; init; }

    protected GearCalculResult()
    {

    }
}
