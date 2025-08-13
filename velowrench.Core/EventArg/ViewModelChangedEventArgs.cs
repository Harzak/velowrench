using velowrench.Core.Interfaces;

namespace velowrench.Core.EventArg;

/// <summary>
/// Provides data for events that occur when the current view model changes during navigation.
/// </summary>
public class ViewModelChangedEventArgs : EventArgs
{
    /// <summary>
    /// Gets the view model that was previously active, or null if this is the first navigation.
    /// </summary>
    public IRoutableViewModel? PreviousViewModel { get; }

    /// <summary>
    /// Gets the view model that is now currently active.
    /// </summary>
    public IRoutableViewModel CurrentViewModel { get; }

    public ViewModelChangedEventArgs(IRoutableViewModel? previous, IRoutableViewModel current)
    {
        PreviousViewModel = previous;
        CurrentViewModel = current ?? throw new ArgumentNullException(nameof(current));
    }
}