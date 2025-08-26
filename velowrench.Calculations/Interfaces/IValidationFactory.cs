using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Calculations.Calculators.Transmission.Chain;
using velowrench.Calculations.Calculators.Transmission.Gear;
using velowrench.Calculations.Calculators.Wheels.SpokeLength;

namespace velowrench.Calculations.Interfaces;

public interface IValidationFactory
{
    ICalculatorInputValidator<ChainLengthCalculatorInput> CreateChainLengthCalculatorInputValidator();
    ICalculatorInputValidator<SpokeLengthCalculatorInput> CreateSpokeLengthCalculatorInputValidator();
    ICalculatorInputValidator<GearCalculatorInput> CreateGearCalculatorInputValidator();
}

