using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Calculations.Calculs.Transmission.Gear;

namespace velowrench.Calculations.Calculs;

/// <summary>
/// Provides a base implementation for calculation result types that contain both the computed output
/// and metadata about the calculation operation.
/// </summary>
public abstract record BaseCalculResult<TInput> where TInput : class
{
    /// <summary>
    /// Gets the UTC timestamp indicating when this calculation was performed.
    /// </summary>
    public required DateTime CalculatedAt { get; init; }

    /// <summary>
    /// Gets the input parameters that were used to produce this calculation result.
    /// </summary>
    public required TInput UsedInputs { get; init; }
}
