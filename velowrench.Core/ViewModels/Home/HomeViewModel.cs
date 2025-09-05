using CommunityToolkit.Mvvm.Input;
using velowrench.Core.Enums;
using velowrench.Core.Interfaces;
using velowrench.Core.ViewModels.Base;

namespace velowrench.Core.ViewModels.Home;

/// <summary>
/// View model for the application's home page that serves as the main navigation hub.
/// </summary>
public sealed partial class HomeViewModel : BaseRoutableViewModel
{
    /// <summary>
    /// Gets the display name of the home view model.
    /// </summary>
    public override string Name { get; }

    public HomeViewModel(
        ILocalizer localizer,
        INavigationService navigationService,
        IToolbar toolbar)
    : base(navigationService, toolbar)
    {
        ArgumentNullException.ThrowIfNull(localizer, nameof(localizer));

        Name = localizer.GetString("VeloWrench");
    }

    public override Task OnResumeAsync()
    {
        Toolbar.ShowProfilAction = () => NavigationService.NavigateToProfileAsync();
        return base.OnResumeAsync();
    }

    public override Task OnInitializedAsync()
    {
        Toolbar.ShowProfilAction = () => NavigationService.NavigateToProfileAsync();
        return base.OnInitializedAsync();
    }

    /// <summary>
    /// Command to navigate to a selected tool based on the tool type.
    /// </summary>
    [RelayCommand]
    private void NavigateToSelectedTool(EVeloWrenchTools toolType)
    {
        base.NavigationService.NavigateToToolAsync(toolType);
    }
}
