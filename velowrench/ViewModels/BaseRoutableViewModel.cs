using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace velowrench.ViewModels;

public abstract class BaseRoutableViewModel : ReactiveObject, IRoutableViewModel
{
    public string? UrlPathSegment { get; }
    public IScreen HostScreen { get; }
    public abstract string Name { get; }
    public ReactiveCommand<Unit, IRoutableViewModel> NavigateBackCommand { get; }

    public BaseRoutableViewModel(IScreen hostScreen)
    {
        this.HostScreen = hostScreen ?? throw new ArgumentNullException(nameof(hostScreen));
        this.UrlPathSegment = Guid.NewGuid().ToString().Substring(0, 5);
        this.NavigateBackCommand = hostScreen.Router.NavigateBack;  
    }
}