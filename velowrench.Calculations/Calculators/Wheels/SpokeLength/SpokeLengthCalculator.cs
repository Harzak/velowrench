using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Calculations.Calculators.Transmission.Chain;
using velowrench.Calculations.Interfaces;
using velowrench.Utils.Results;

namespace velowrench.Calculations.Calculators.Wheels.SpokeLength;

public sealed class SpokeLengthCalculator : BaseCalculator<SpokeLengthCalculatorInput, SpokeLengthCalculatorResult>
{
    protected override string CalculatorName => nameof(SpokeLengthCalculator);

    public SpokeLengthCalculator(Func<ICalculatorInputValidation<SpokeLengthCalculatorInput>> validationProvider, ILogger logger) : base(validationProvider, logger)
    {

    }

    protected override OperationResult<SpokeLengthCalculatorResult> Calculate(SpokeLengthCalculatorInput input)
    {
        throw new NotImplementedException();
    }
}
