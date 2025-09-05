using Microsoft.Extensions.Logging;
using velowrench.Calculations.Interfaces;

namespace velowrench.Calculations.Calculators.Wheels.SpokeLength;

/// <summary>
/// Factory for creating instances of <see cref="SpokeLengthCalculator"/>.
/// </summary>
public sealed class SpokeLengthCalculatorFactory : ICalculatorFactory<SpokeLengthCalculatorInput, SpokeLengthCalculatorResult>
{
    private readonly ICalculatorInputValidator<SpokeLengthCalculatorInput> _inputValidator;
    private readonly IUnitStore _unitStore;
    private readonly ILogger _logger;

    public SpokeLengthCalculatorFactory(
        ICalculatorInputValidator<SpokeLengthCalculatorInput> inputValidator,
        IUnitStore unitStore,
        ILogger<SpokeLengthCalculatorFactory> logger)
    {
        _inputValidator = inputValidator ?? throw new ArgumentNullException(nameof(inputValidator));
        _unitStore = unitStore ?? throw new ArgumentNullException(nameof(unitStore));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Creates and returns an instance of a spoke length calculator.
    /// </summary>
    public ICalculator<SpokeLengthCalculatorInput, SpokeLengthCalculatorResult> CreateCalculator()
    {
        return new SpokeLengthCalculator(_inputValidator, _unitStore, _logger);
    }
}
