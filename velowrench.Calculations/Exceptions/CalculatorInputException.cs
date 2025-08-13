using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace velowrench.Calculations.Exceptions;

/// <summary>
/// Exception thrown when invalid input data is provided to a calculation.
/// </summary>
public class CalculatorInputException : Exception
{
    public CalculatorInputException()  : base("Invalid input for calculation.")
    {

    }

    public CalculatorInputException(string message) : base(message)
    {

    }

    public CalculatorInputException(IEnumerable<string> messages) : base(string.Join(Environment.NewLine, messages))
    {

    }

    public CalculatorInputException(string message, Exception innerException)  : base(message, innerException)
    {

    }

    /// <summary>
    /// Throws a <see cref="CalculatorInputException"/> if the specified argument is negative or zero.
    /// </summary>
    /// <param name="argument">The value to check.</param>
    /// <param name="paramName">The name of the parameter being validated (automatically captured).</param>
    /// <exception cref="CalculatorInputException">Thrown when <paramref name="argument"/> is less than or equal to zero.</exception>
    public static void ThrowIfNegativeOrZero(double argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
        if (argument <= 0)
        {
            throw new CalculatorInputException($"The value of '{paramName}' must be greater than zero. Current value: {argument}");
        }
    }


    /// <summary>
    /// Throws a <see cref="CalculatorInputException"/> if the specified argument is negative.
    /// </summary>
    /// <param name="argument">The value to check.</param>
    /// <param name="paramName">The name of the parameter being validated (automatically captured).</param>
    /// <exception cref="CalculatorInputException">Thrown when <paramref name="argument"/> is less than zero.</exception>
    public static void ThrowIfNegative(double argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
        if (argument < 0)
        {
            throw new CalculatorInputException($"The value of '{paramName}' must be greater than zero. Current value: {argument}");
        }
    }
}
