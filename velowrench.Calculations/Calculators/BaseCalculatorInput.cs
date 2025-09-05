using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Calculations.Constants;

namespace velowrench.Calculations.Calculators;

public abstract class BaseCalculatorInput
{
    /// <summary>
    /// Gets the number of decimal places to include in calculation results.
    /// Controls the precision of the output values.
    /// </summary>
    public int Precision { get; set; }

    protected BaseCalculatorInput() : this(CalculationConstants.DEFAULT_PRECISION)
    {
        
    }

    protected BaseCalculatorInput(int precision)
    {
        this.Precision = precision;
    }
}
