using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Calculations.Calculs.Transmission.Chain;
using velowrench.Calculations.Interfaces;

namespace velowrench.Calculations.Calculs.Transmission.Gear;

public class GearCalculFactory : ICalculFactory<GearCalculInput, GearCalculResult>
{
    private readonly Func<ICalculInputValidation<GearCalculInput>> _validationProvider;
    private readonly ILogger _logger;

    public GearCalculFactory(Func<ICalculInputValidation<GearCalculInput>> validationProvider, ILogger<GearCalculFactory> logger)
    {
        _validationProvider = validationProvider ?? throw new ArgumentNullException(nameof(validationProvider));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Creates a new gear calculation instance.
    /// </summary>
    /// <returns>A new <see cref="GearCalcul"/> instance ready to perform calculations.</returns>
    public ICalcul<GearCalculInput, GearCalculResult> CreateCalcul()
    {
        return new GearCalcul(_validationProvider, _logger);
    }
}
