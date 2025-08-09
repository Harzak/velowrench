namespace velowrench.Core.Interfaces;

public interface IRoutableViewModel
{
    public string Name { get; }
    public string UrlPathSegment { get; }
    public bool CanShowHelpPage { get; }

    void ShowHelpPage();
}
