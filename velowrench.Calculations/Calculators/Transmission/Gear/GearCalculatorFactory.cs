using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Calculations.Calculators.Transmission.Chain;
using velowrench.Calculations.Interfaces;

namespace velowrench.Calculations.Calculators.Transmission.Gear;

/// <summary>
/// Factory for creating instances of<see cref = "GearCalculator"/>.
/// </summary>
public class GearCalculatorFactory : ICalculatorFactory<GearCalculatorInput, GearCalculatorResult>
{
    private readonly Func<ICalculatorInputValidation<GearCalculatorInput>> _validationProvider;
    private readonly ILogger _logger;

    public GearCalculatorFactory(Func<ICalculatorInputValidation<GearCalculatorInput>> validationProvider, ILogger<GearCalculatorFactory> logger)
    {
        _validationProvider = validationProvider ?? throw new ArgumentNullException(nameof(validationProvider));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Creates a new gear calculation instance ready to perform calculations.
    /// Each created instance is independent and can be used for multiple calculations.
    /// </summary>
    public ICalculator<GearCalculatorInput, GearCalculatorResult> CreateCalculator()
    {
        return new GearCalculator(_validationProvider, _logger);
    }
}
