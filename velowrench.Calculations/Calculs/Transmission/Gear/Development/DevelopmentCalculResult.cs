using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace velowrench.Calculations.Calculs.Transmission.Gear.Development;

public sealed record DevelopmentCalculResult : GearCalculResult
{
    /// <summary>
    /// Gets the input parameters that were used to produce this calculation result.
    /// </summary>
    public required DevelopmentCalculInput UsedInputs { get; init; }

    public DevelopmentCalculResult(): base()
    {
    }
}
