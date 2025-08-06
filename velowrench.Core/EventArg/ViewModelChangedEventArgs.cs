using velowrench.Core.Interfaces;

namespace velowrench.Core.EventArg;

public class ViewModelChangedEventArgs : EventArgs
{
    public IRoutableViewModel? PreviousViewModel { get; }
    public IRoutableViewModel CurrentViewModel { get; }

    public ViewModelChangedEventArgs(IRoutableViewModel? previous, IRoutableViewModel current)
    {
        PreviousViewModel = previous;
        CurrentViewModel = current;
    }
}