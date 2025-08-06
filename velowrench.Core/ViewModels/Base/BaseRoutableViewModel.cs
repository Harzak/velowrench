using CommunityToolkit.Mvvm.ComponentModel;
using velowrench.Core.Interfaces;
using velowrench.Core.Services;

namespace velowrench.Core.ViewModels.Base;

public abstract partial class BaseRoutableViewModel : ObservableObject,  IRoutableViewModel
{
    protected INavigationService NavigationService { get; }
    public string UrlPathSegment { get; }
    public abstract string Name { get; }

    public BaseRoutableViewModel(INavigationService  navigationService)
    {
        NavigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));    
        UrlPathSegment = Guid.NewGuid().ToString().Substring(0, 5);
    }
}