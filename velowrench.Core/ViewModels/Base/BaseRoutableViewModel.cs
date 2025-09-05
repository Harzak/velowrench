using CommunityToolkit.Mvvm.ComponentModel;
using velowrench.Core.Interfaces;

namespace velowrench.Core.ViewModels.Base;

/// <summary>
/// Abstract base class for view models that can be navigated to and support routing functionality.
/// </summary>
public abstract partial class BaseRoutableViewModel : ObservableValidator, IRoutableViewModel
{
    /// <inheritdoc/>
    protected INavigationService NavigationService { get; }

    /// <inheritdoc/>
    public string UrlPathSegment { get; }

    /// <inheritdoc/>
    public abstract string Name { get; }

    /// <inheritdoc/>
    public IToolbar Toolbar { get; }

    protected BaseRoutableViewModel(INavigationService navigationService, IToolbar toolbar)
    {
        this.NavigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        this.UrlPathSegment = Guid.NewGuid().ToString()[..5];
        this.Toolbar = toolbar ?? throw new ArgumentNullException(nameof(toolbar));
    }

    /// <inheritdoc/>
    public virtual Task OnInitializedAsync()
    {
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public virtual Task OnResumeAsync()
    {
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public virtual Task OnDestroyAsync()
    {
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public virtual Task OnForceDestroyAsync()
    {
        return Task.CompletedTask;
    }

    protected virtual void Dispose(bool disposing)
    {

    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}