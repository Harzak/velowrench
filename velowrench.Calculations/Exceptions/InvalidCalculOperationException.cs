using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using velowrench.Calculations.Interfaces;

namespace velowrench.Calculations.Exceptions;

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

    public static void ThrowIfCalculInProgress([NotNull] ICalcul argument)
    {
        if (argument.State == Utils.Enums.ECalculState.InProgress)
        {
            throw new InvalidCalculOperationException($"The calculation is already in progress. Cannot change the state while it is running.");
        }
    }
}
