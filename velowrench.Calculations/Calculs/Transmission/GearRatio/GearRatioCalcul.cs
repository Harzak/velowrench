using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Utils.Results;

namespace velowrench.Calculations.Calculs.Transmission.GearRatio;

public class GearRatioCalcul : BaseCalcul<GearRatioCalculInput, GearRatioCalculResult>
{
    protected override string CalculName => nameof(GearRatioCalcul);

    public GearRatioCalcul(ILogger logger) : base(logger)
    {
    }

    protected override OperationResult<GearRatioCalculResult> Calculate(GearRatioCalculInput input)
    {
        
    }
}
