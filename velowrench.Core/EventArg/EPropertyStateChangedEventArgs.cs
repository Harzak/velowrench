using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Utils.Enums;

namespace velowrench.Core.EventArg;

/// <summary>
/// Event arguments for validation state changes.
/// </summary>
public class EPropertyStateChangedEventArgs : EventArgs
{
    /// <summary>
    /// Gets the name of the property whose validation state changed.
    /// </summary>
    public string PropertyName { get; }

    /// <summary>
    /// Gets the previous validation state.
    /// </summary>
    public EPropertyState PreviousState { get; }

    /// <summary>
    /// Gets the new validation state.
    /// </summary>
    public EPropertyState NewState { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="EPropertyStateChangedEventArgs"/> class.
    /// </summary>
    /// <param name="propertyName">The name of the property whose state changed.</param>
    /// <param name="previousState">The previous validation state.</param>
    /// <param name="newState">The new validation state.</param>
    public EPropertyStateChangedEventArgs(string propertyName, EPropertyState previousState, EPropertyState newState)
    {
        PropertyName = propertyName;
        PreviousState = previousState;
        NewState = newState;
    }
}

