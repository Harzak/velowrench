using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using velowrench.Core.Enums;
using velowrench.Core.Interfaces;
using velowrench.Core.Services;
using velowrench.Core.ViewModels.Base;

namespace velowrench.Core.ViewModels.Home;

public partial class HomeViewModel : BaseRoutableViewModel
{
    public override string Name { get; }

    public HomeViewModel(ILocalizer localizer, INavigationService navigationService) : base(navigationService)
    {
        Name = localizer.GetString("home");
    }

    [RelayCommand]
    private void NavigateToSelectedTool(EVeloWrenchTools toolType)
    {
        base.NavigationService.NavigateToTool(toolType);
    }
}
