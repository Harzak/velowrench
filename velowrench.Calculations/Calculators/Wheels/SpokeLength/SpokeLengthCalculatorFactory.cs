using Microsoft.Extensions.Logging;
using velowrench.Calculations.Interfaces;

namespace velowrench.Calculations.Calculators.Wheels.SpokeLength;

/// <summary>
/// Factory for creating instances of <see cref="SpokeLengthCalculator"/>.
/// </summary>
public sealed class SpokeLengthCalculatorFactory : ICalculatorFactory<SpokeLengthCalculatorInput, SpokeLengthCalculatorResult>
{
    private readonly Func<ICalculatorInputValidation<SpokeLengthCalculatorInput>> _validationProvider;
    private readonly ILogger _logger;

    public SpokeLengthCalculatorFactory(Func<ICalculatorInputValidation<SpokeLengthCalculatorInput>> validationProvider, ILogger<SpokeLengthCalculatorFactory> logger)
    {
        _validationProvider = validationProvider ?? throw new ArgumentNullException(nameof(validationProvider));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Creates and returns an instance of a spoke length calculator.
    /// </summary>
    public ICalculator<SpokeLengthCalculatorInput, SpokeLengthCalculatorResult> CreateCalculator()
    {
        return new SpokeLengthCalculator(_validationProvider, _logger);
    }
}
