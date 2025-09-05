namespace velowrench.Core.EventArg;

public class ViewModelRegistryItemEventArgs : EventArgs
{
    public Guid Identifier { get; }

    public ViewModelRegistryItemEventArgs(Guid identifier)
    {
        this.Identifier = identifier;
    }
}
