using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Calculations.Interfaces;

namespace velowrench.Calculations.Calculs.Transmission.ChainLength;

public class ChainLengthCalculFactory : ICalculFactory<ChainLengthCalculInput, ChainLengthCalculResult>
{
    private readonly ILogger _logger;

    public ChainLengthCalculFactory(ILogger<ChainLengthCalculFactory> logger)
    {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public ICalcul<ChainLengthCalculInput, ChainLengthCalculResult> CreateCalcul()
    {
        return new ChainLengthCalcul(_logger);
    }
}