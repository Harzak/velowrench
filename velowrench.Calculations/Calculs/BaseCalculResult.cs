using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Calculations.Calculs.Transmission.Gear;

namespace velowrench.Calculations.Calculs;

public abstract record BaseCalculResult<TInput> where TInput : class
{
    /// <summary>
    /// Gets the UTC timestamp when this calculation was performed.
    /// </summary>
    public required DateTime CalculatedAt { get; init; }
    public required TInput UsedInputs { get; init; }
}
