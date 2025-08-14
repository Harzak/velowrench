using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Calculations.Enums;

namespace velowrench.Calculations.Calculators.Transmission.Gear;

public static class GearCalculatorResultInterpreter
{
    public static EGearCalculationResultIntensity DetermineIntensity(double value, EGearCalculatorType calculatorType)
    {
        switch (calculatorType)
        {
            case EGearCalculatorType.GainRatio:
                if (value < 1.0)
                    return EGearCalculationResultIntensity.Low;
                else if (value < 2.0)
                    return EGearCalculationResultIntensity.Medium;
                else
                    return EGearCalculationResultIntensity.High;
            case EGearCalculatorType.GearInches:
                if (value < 50)
                    return EGearCalculationResultIntensity.High;
                else if (value < 80)
                    return EGearCalculationResultIntensity.Medium;
                else
                    return EGearCalculationResultIntensity.Low;
            case EGearCalculatorType.Speed:
                if (value < 10)
                    return EGearCalculationResultIntensity.Low;
                else if (value < 20)
                    return EGearCalculationResultIntensity.Medium;
                else
                    return EGearCalculationResultIntensity.High;
            case EGearCalculatorType.Development://in yards
                if (value < 50)
                    return EGearCalculationResultIntensity.Low;
                else if (value < 100)
                    return EGearCalculationResultIntensity.Medium;
                else
                    return EGearCalculationResultIntensity.High;
            default:
                throw new ArgumentOutOfRangeException(nameof(calculatorType), "Unknown gear calculator type.");
        }
    }
}
