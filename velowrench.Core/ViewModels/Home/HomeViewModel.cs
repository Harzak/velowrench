using DynamicData.Binding;
using ReactiveUI;
using System;
using System.Reactive;
using velowrench.Core.Enums;
using velowrench.Core.Interfaces;

namespace velowrench.Core.ViewModels.Home;

public class HomeViewModel : ReactiveObject, IRoutableViewModel
{
    private readonly IToolsViewModelFactory _toolsViewModelFactory;
    public IScreen HostScreen { get; }
    public string? UrlPathSegment { get; }
    public ReactiveCommand<EVeloWrenchTools, Unit> NavigateToSelectedToolCommand { get; }

    public HomeViewModel(IScreen hostScreen, IToolsViewModelFactory toolsViewModelFactory)
    {
        _toolsViewModelFactory = toolsViewModelFactory ?? throw new ArgumentNullException(nameof(toolsViewModelFactory));
        HostScreen = hostScreen ?? throw new ArgumentNullException(nameof(hostScreen));
        UrlPathSegment = Guid.NewGuid().ToString().Substring(0, 5);

        NavigateToSelectedToolCommand = ReactiveCommand.Create<EVeloWrenchTools>(NavigateToTool);
    }

    private void NavigateToTool(EVeloWrenchTools toolType)
    {
        IRoutableViewModel viewModel = _toolsViewModelFactory.CreateRoutableViewModel(toolType, HostScreen);
        HostScreen.Router.Navigate.Execute(viewModel);
    }
}
