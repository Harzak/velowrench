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

