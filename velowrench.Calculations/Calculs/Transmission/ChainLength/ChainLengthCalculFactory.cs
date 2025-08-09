using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Calculations.Interfaces;

namespace velowrench.Calculations.Calculs.Transmission.ChainLength;

/// <summary>
/// Factory for creating chain length calculation instances.
/// </summary>
public sealed class ChainLengthCalculFactory : ICalculFactory<ChainLengthCalculInput, ChainLengthCalculResult>
{
    private readonly ILogger _logger;

    public ChainLengthCalculFactory(ILogger<ChainLengthCalculFactory> logger)
    {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Creates a new chain length calculation instance.
    /// </summary>
    /// <returns>A new <see cref="ChainLengthCalcul"/> instance ready to perform calculations.</returns>
    public ICalcul<ChainLengthCalculInput, ChainLengthCalculResult> CreateCalcul()
    {
        return new ChainLengthCalcul(_logger);
    }
}