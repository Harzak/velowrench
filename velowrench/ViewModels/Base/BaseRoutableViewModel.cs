using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace velowrench.ViewModels.Base;

public abstract class BaseRoutableViewModel : ReactiveObject, IRoutableViewModel
{
    public string? UrlPathSegment { get; }
    public IScreen HostScreen { get; }
    public abstract string Name { get; }
    public ReactiveCommand<Unit, IRoutableViewModel> NavigateBackCommand { get; }

    public BaseRoutableViewModel(IScreen hostScreen)
    {
        HostScreen = hostScreen ?? throw new ArgumentNullException(nameof(hostScreen));
        UrlPathSegment = Guid.NewGuid().ToString().Substring(0, 5);
        NavigateBackCommand = hostScreen.Router.NavigateBack;  
    }
}