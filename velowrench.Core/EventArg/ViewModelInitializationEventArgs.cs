namespace velowrench.Core.EventArg;

public sealed class ViewModelInitializationEventArgs : EventArgs
{
    public bool IsInitializing { get; }
    public string UrlPathSegment { get; }

    public ViewModelInitializationEventArgs(bool isInitializing, string urlPathSegment)
    {
        IsInitializing = isInitializing;
        UrlPathSegment = urlPathSegment;
    }
}

