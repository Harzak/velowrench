using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Calculations.Calculators.Transmission.Chain;
using velowrench.Calculations.Interfaces;

namespace velowrench.Calculations.Calculators.Wheels.SpokeLength;

public sealed class SpokeLengthCalculatorFactory : ICalculatorFactory<SpokeLengthCalculatorInput, SpokeLengthCalculatorResult>
{
    private readonly Func<ICalculatorInputValidation<SpokeLengthCalculatorInput>> _validationProvider;
    private readonly ILogger _logger;

    public SpokeLengthCalculatorFactory(Func<ICalculatorInputValidation<SpokeLengthCalculatorInput>> validationProvider, ILogger<SpokeLengthCalculatorFactory> logger)
    {
        _validationProvider = validationProvider ?? throw new ArgumentNullException(nameof(validationProvider));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public ICalculator<SpokeLengthCalculatorInput, SpokeLengthCalculatorResult> CreateCalculator()
    {
        return new SpokeLengthCalculator(_validationProvider, _logger);
    }
}
