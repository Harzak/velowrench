using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Calculations.Calculs.Transmission.Chain;
using velowrench.Calculations.Interfaces;

namespace velowrench.Calculations.Calculs.Transmission.Gear;

/// <summary>
/// Factory for creating gear calculation instances.
/// Implements the factory pattern to provide properly configured <see cref="GearCalcul"/> instances
/// with required dependencies for validation and logging.
/// </summary>
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
    /// Creates a new gear calculation instance ready to perform calculations.
    /// Each created instance is independent and can be used for multiple calculations.
    /// </summary>
    public ICalcul<GearCalculInput, GearCalculResult> CreateCalcul()
    {
        return new GearCalcul(_validationProvider, _logger);
    }
}
