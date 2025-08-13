using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using velowrench.Calculations.Interfaces;

namespace velowrench.Calculations.Exceptions;

/// <summary>
/// Exception thrown when an invalid operation is attempted on a calculation.
/// </summary>
public class InvalidCalculatorOperationException : Exception
{
    public InvalidCalculatorOperationException()
        : base("Invalid operation for the calculation.")
    {

    }

    public InvalidCalculatorOperationException(string message) : base(message)
    {

    }

    public InvalidCalculatorOperationException(string message, Exception innerException) : base(message, innerException)
    {

    }

    /// <summary>
    /// Throws an <see cref="InvalidCalculatorOperationException"/> if the specified calculation is currently in progress.
    /// </summary>
    /// <exception cref="InvalidCalculatorOperationException">Thrown when the calculation is in progress.</exception>
    /// <remarks>
    /// This method is typically used to prevent starting a new calculation when one is already running,
    /// ensuring thread safety and proper calculation state management.
    /// </remarks>
    public static void ThrowIfCalculInProgress([NotNull] ICalculator argument)
    {
        if (argument.State == Utils.Enums.ECalculatorState.InProgress)
        {
            throw new InvalidCalculatorOperationException($"The calculation is already in progress. Cannot change the state while it is running.");
        }
    }
}
