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
public class InvalidCalculOperationException : Exception
{
    public InvalidCalculOperationException()
        : base("Invalid operation for the calculation.")
    {

    }

    public InvalidCalculOperationException(string message) : base(message)
    {

    }

    public InvalidCalculOperationException(string message, Exception innerException) : base(message, innerException)
    {

    }

    /// <summary>
    /// Throws an <see cref="InvalidCalculOperationException"/> if the specified calculation is currently in progress.
    /// </summary>
    /// <exception cref="InvalidCalculOperationException">Thrown when the calculation is in progress.</exception>
    /// <remarks>
    /// This method is typically used to prevent starting a new calculation when one is already running,
    /// ensuring thread safety and proper calculation state management.
    /// </remarks>
    public static void ThrowIfCalculInProgress([NotNull] ICalcul argument)
    {
        if (argument.State == Utils.Enums.ECalculState.InProgress)
        {
            throw new InvalidCalculOperationException($"The calculation is already in progress. Cannot change the state while it is running.");
        }
    }
}
