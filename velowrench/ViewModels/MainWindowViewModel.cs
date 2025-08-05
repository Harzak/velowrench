using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using velowrench.Interfaces;

namespace velowrench.ViewModels;

public class MainWindowViewModel : ReactiveObject, IScreen
{
    public RoutingState Router { get; }

    public MainWindowViewModel(ILocalizer localizer)
    {
        this.Router = new RoutingState();
        Router.Navigate.Execute(new HomeViewModel(this, localizer));
    }
}
