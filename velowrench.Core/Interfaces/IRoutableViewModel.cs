namespace velowrench.Core.Interfaces;

/// <summary>
/// Interface for view models that can be navigated to and managed by the navigation service.
/// </summary>
public interface IRoutableViewModel
{
    /// <summary>
    /// Gets the display name of the view model for UI presentation.
    /// </summary>
    public string Name { get; }
    
    /// <summary>
    /// Gets the URL path segment identifier for this view model.
    /// </summary>
    public string UrlPathSegment { get; }
    
    /// <summary>
    /// Gets a value indicating whether this view model has an associated help page.
    /// </summary>
    public bool CanShowHelpPage { get; }

    /// <summary>
    /// Shows the help page associated with this view model.
    /// </summary>
    void ShowHelpPage();
}
