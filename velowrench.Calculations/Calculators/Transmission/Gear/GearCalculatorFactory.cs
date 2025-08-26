using Microsoft.Extensions.Logging;
using velowrench.Calculations.Calculators.Wheels.SpokeLength;
using velowrench.Calculations.Interfaces;
using velowrench.Core.Validation.Pipeline;

namespace velowrench.Calculations.Calculators.Transmission.Gear;

/// <summary>
/// Factory for creating instances of<see cref = "GearCalculator"/>.
/// </summary>
public class GearCalculatorFactory : ICalculatorFactory<GearCalculatorInput, GearCalculatorResult>
{
    private readonly ICalculatorInputValidator<GearCalculatorInput> _inputValidator;
    private readonly ILogger _logger;

    public GearCalculatorFactory(ICalculatorInputValidator<GearCalculatorInput> inputValidator, ILogger<GearCalculatorFactory> logger)
    {
        _inputValidator = inputValidator ?? throw new ArgumentNullException(nameof(inputValidator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Creates a new gear calculation instance ready to perform calculations.
    /// Each created instance is independent and can be used for multiple calculations.
    /// </summary>
    public ICalculator<GearCalculatorInput, GearCalculatorResult> CreateCalculator()
    {
        return new GearCalculator(_inputValidator, _logger);
    }
}
