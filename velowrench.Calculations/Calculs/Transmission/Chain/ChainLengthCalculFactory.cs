using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Calculations.Calculs.Transmission.Gear;
using velowrench.Calculations.Interfaces;

namespace velowrench.Calculations.Calculs.Transmission.Chain;

/// <summary>
/// Factory for creating chain length calculation instances.
/// </summary>
public sealed class ChainLengthCalculFactory : ICalculFactory<ChainLengthCalculInput, ChainLengthCalculResult>
{
    private readonly Func<ICalculInputValidation<ChainLengthCalculInput>> _validationProvider;
    private readonly ILogger _logger;

    public ChainLengthCalculFactory(Func<ICalculInputValidation<ChainLengthCalculInput>> validationProvider, ILogger<ChainLengthCalculFactory> logger)
    {
        _validationProvider = validationProvider ?? throw new ArgumentNullException(nameof(validationProvider));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Creates a new chain length calculation instance.
    /// </summary>
    /// <returns>A new <see cref="ChainLengthCalcul"/> instance ready to perform calculations.</returns>
    public ICalcul<ChainLengthCalculInput, ChainLengthCalculResult> CreateCalcul()
    {
        return new ChainLengthCalcul(_validationProvider, _logger);
    }
}