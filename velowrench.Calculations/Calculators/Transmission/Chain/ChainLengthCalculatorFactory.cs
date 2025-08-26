using Microsoft.Extensions.Logging;
using velowrench.Calculations.Interfaces;

namespace velowrench.Calculations.Calculators.Transmission.Chain;

/// <summary>
/// Factory for creating instances of <see cref="ChainLengthCalculator"/>.
/// </summary>
public sealed class ChainLengthCalculatorFactory : ICalculatorFactory<ChainLengthCalculatorInput, ChainLengthCalculatorResult>
{
    private readonly ICalculatorInputValidator<ChainLengthCalculatorInput> _inputValidator;
    private readonly ILogger _logger;

    public ChainLengthCalculatorFactory(ICalculatorInputValidator<ChainLengthCalculatorInput> inputValidator, ILogger<ChainLengthCalculatorFactory> logger)
    {
        _inputValidator = inputValidator ?? throw new ArgumentNullException(nameof(inputValidator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>   
    /// Creates a new chain length calculation instance.
    /// </summary>
    /// <returns>A new <see cref="ChainLengthCalculator"/> instance ready to perform calculations.</returns>
    public ICalculator<ChainLengthCalculatorInput, ChainLengthCalculatorResult> CreateCalculator()
    {
        return new ChainLengthCalculator(_inputValidator, _logger);
    }
}