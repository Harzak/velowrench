using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Core.EventArg;
using velowrench.Core.Navigation.Context;

namespace velowrench.Core.Interfaces;

public interface INavigationHandler : IDisposable
{
    IRoutableViewModel? ActiveViewModel { get; }
    public bool CanPop { get; }

    Task ClearAsync();
    Task PushAsync(IRoutableViewModel viewModel, NavigationContext context);
    Task PopAsync(NavigationContext context);

    event EventHandler<EventArgs>? ActiveViewModelChanged;
}