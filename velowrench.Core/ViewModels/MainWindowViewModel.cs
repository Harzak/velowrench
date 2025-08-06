using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using velowrench.Core.Interfaces;
using velowrench.Core.ViewModels.Home;

namespace velowrench.Core.ViewModels;

public class MainWindowViewModel : ReactiveObject, IScreen
{
    public RoutingState Router { get; }

    public MainWindowViewModel(IToolsViewModelFactory toolsViewModelFactory)
    {
        this.Router = new RoutingState();
        Router.Navigate.Execute(new HomeViewModel(this, toolsViewModelFactory));
    }
}
