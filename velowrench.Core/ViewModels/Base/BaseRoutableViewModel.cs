using CommunityToolkit.Mvvm.ComponentModel;
using velowrench.Core.Interfaces;

namespace velowrench.Core.ViewModels.Base;

/// <summary>
/// Abstract base class for view models that can be navigated to and support routing functionality.
/// </summary>
public abstract partial class BaseRoutableViewModel : ObservableValidator, IRoutableViewModel, IDisposable
{
    private bool _disposedValue;

    protected INavigationService NavigationService { get; }

    /// <summary>
    /// Gets the unique URL path segment identifier for this view model.
    /// </summary>
    public string UrlPathSegment { get; }

    /// <summary>
    /// Gets a value indicating whether this view model has an associated help page.
    /// </summary>
    public virtual bool CanShowHelpPage { get; }

    /// <summary>
    /// Gets the display name of this view model.
    /// </summary>
    public abstract string Name { get; }

    protected BaseRoutableViewModel(INavigationService navigationService)
    {
        NavigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        UrlPathSegment = Guid.NewGuid().ToString().Substring(0, 5);
    }

    /// <summary>
    /// Shows the help page associated with this view model.
    /// </summary>
    public virtual void ShowHelpPage()
    {

    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            _disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}