using CommunityToolkit.Mvvm.ComponentModel;
using DynamicData.Binding;
using ReactiveUI;
using System;
using System.Reactive;
using velowrench.Interfaces;

namespace velowrench.ViewModels;

public class HomeViewModel : ReactiveObject, IRoutableViewModel
{
    public ReactiveCommand<Unit, Unit> NavigateToChainLengthCalculatorCommand { get; }

    public string? UrlPathSegment => "home";

    public IScreen HostScreen { get; }

    public HomeViewModel(IScreen hostScreen, ILocalizer localizer)
    {
        this.HostScreen = hostScreen ?? throw new ArgumentNullException(nameof(hostScreen));

        this.NavigateToChainLengthCalculatorCommand = ReactiveCommand.Create(() =>
        {
            hostScreen.Router.Navigate.Execute(new ChainLengthCalculatorViewModel(hostScreen, localizer));
        });
    }
}
