using CommunityToolkit.Mvvm.ComponentModel;
using velowrench.Core.Interfaces;

namespace velowrench.Core.ViewModels.Base;

public abstract partial class BaseRoutableViewModel : ObservableValidator, IRoutableViewModel, IDisposable
{
    private bool _disposedValue;

    protected INavigationService NavigationService { get; }
    public string UrlPathSegment { get; }
    public virtual bool CanShowHelpPage { get; }

    public abstract string Name { get; }

    public BaseRoutableViewModel(INavigationService navigationService)
    {
        NavigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        UrlPathSegment = Guid.NewGuid().ToString().Substring(0, 5);
    }

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