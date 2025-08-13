using Microsoft.Extensions.Logging;
using velowrench.Calculations.Interfaces;

namespace velowrench.Calculations.Calculators.Transmission.Chain;

/// <summary>
/// Factory for creating chain length calculation instances.
/// </summary>
public sealed class ChainLengthCalculatorFactory : ICalculatorFactory<ChainLengthCalculatorInput, ChainLengthCalculatorResult>
{
    private readonly Func<ICalculatorInputValidation<ChainLengthCalculatorInput>> _validationProvider;
    private readonly ILogger _logger;

    public ChainLengthCalculatorFactory(Func<ICalculatorInputValidation<ChainLengthCalculatorInput>> validationProvider, ILogger<ChainLengthCalculatorFactory> logger)
    {
        _validationProvider = validationProvider ?? throw new ArgumentNullException(nameof(validationProvider));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Creates a new chain length calculation instance.
    /// </summary>
    /// <returns>A new <see cref="ChainLengthCalculator"/> instance ready to perform calculations.</returns>
    public ICalculator<ChainLengthCalculatorInput, ChainLengthCalculatorResult> CreateCalculator()
    {
        return new ChainLengthCalculator(_validationProvider, _logger);
    }
}