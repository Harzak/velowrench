using CommunityToolkit.Mvvm.ComponentModel;
using velowrench.Core.Interfaces;

namespace velowrench.Core.ViewModels.Base;

/// <summary>
/// Abstract base class for view models that can be navigated to and support routing functionality.
/// </summary>
public abstract partial class BaseRoutableViewModel : ObservableValidator, IRoutableViewModel
{
    protected INavigationService NavigationService { get; }

    /// <inheritdoc/>
    public string UrlPathSegment { get; }

    /// <inheritdoc/>
    public virtual bool CanShowHelpPage { get; }

    /// <inheritdoc/>
    public abstract string Name { get; }

    /// <inheritdoc/>
    public virtual bool CanShowContextMenu { get; }

    protected BaseRoutableViewModel(INavigationService navigationService)
    {
        NavigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        UrlPathSegment = Guid.NewGuid().ToString()[..5];
    }

    /// <inheritdoc/>
    public virtual void ShowHelpPage()
    {

    }

    /// <inheritdoc/>
    public virtual void ResetToDefault()
    {

    }

    public virtual Task OnInitializedAsync()
    {
        return Task.CompletedTask;
    }

    public virtual Task OnDestroyAsync()
    {
        return Task.CompletedTask;
    }

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