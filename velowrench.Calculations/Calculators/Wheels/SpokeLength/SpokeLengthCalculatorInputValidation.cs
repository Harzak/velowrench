using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Calculations.Interfaces;

namespace velowrench.Calculations.Calculators.Wheels.SpokeLength;

internal sealed class SpokeLengthCalculatorInputValidation : ICalculatorInputValidation<SpokeLengthCalculatorInput>
{
    public IEnumerable<string> ErrorMessages => throw new NotImplementedException();

    public bool Validate(SpokeLengthCalculatorInput input)
    {
        throw new NotImplementedException();
    }
}
