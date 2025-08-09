using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Utils.Enums;
using velowrench.Utils.EventArg;
using velowrench.Utils.Results;

namespace velowrench.Calculations.Interfaces;

/// <summary>
/// Base interface for all calculations, providing state tracking and event notification capabilities.
/// </summary>
public interface ICalcul 
{
    /// <summary>
    /// Gets the current state of the calculation.
    /// </summary>
    /// <value>
    /// The current calculation state indicating whether the calculation is not started, in progress, completed, or failed.
    /// </value>
    public ECalculState State { get;  }

    /// <summary>
    /// Event raised when the calculation state changes.
    /// </summary>
    /// <remarks>
    /// Subscribers can monitor this event to track calculation progress and respond to state changes.
    /// </remarks>
    event EventHandler<CalculStateEventArgs>? StateChanged;
}

