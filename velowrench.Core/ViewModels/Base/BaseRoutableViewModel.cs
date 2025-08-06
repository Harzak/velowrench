using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Core.Interfaces;
using velowrench.Core.Services;

namespace velowrench.Core.ViewModels.Base;

public abstract class BaseRoutableViewModel : ObservableObject,  IRoutableViewModel
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