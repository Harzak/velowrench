using Avalonia;
using Avalonia.Styling;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using UnitsNet.Units;
using velowrench.Calculations.Interfaces;
using velowrench.Core.Configuration;
using velowrench.Core.Interfaces;
using velowrench.Core.ViewModels.Base;
using velowrench.Core.ViewModels.Help;
using velowrench.Repository.Models;
using velowrench.Repository.Repositories;
using velowrench.Utils.Results;

namespace velowrench.Core.ViewModels.Home;

/// <summary>
/// Represents the view model for managing user profile settings, including culture, theme, and unit preferences.
/// </summary>
public sealed partial class ProfileViewModel : BaseRoutableViewModel
{
    private readonly IUnitStore _unitStore;
    private readonly ILocalizer _localizer;

    /// <inheritdoc/>
    public override string Name { get; }

    public IAppConfiguration AppConfig { get; }
    public IReadOnlyCollection<LengthUnit> LengthAvailableUnits => _unitStore.LengthAvailableUnits;
    public IReadOnlyCollection<LengthUnit> DistanceAvailableUnits => _unitStore.DistanceAvailableUnits;
    public IReadOnlyCollection<SpeedUnit> SpeedAvailableUnits => _unitStore.SpeedAvailableUnits;

    public ProfileViewModel(
        IAppConfiguration appConfig,
        ILocalizer localizer,
        IUnitStore unitStore,
        INavigationService navigationService,
        IToolbar toolbar)
    : base(navigationService, toolbar)
    {
        _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        _unitStore = unitStore ?? throw new ArgumentNullException(nameof(unitStore));
        this.AppConfig = appConfig ?? throw new ArgumentNullException(nameof(appConfig));

        this.Name = _localizer.GetString("Settings");
    }

    public async override Task OnDestroyAsync()
    {
        await this.AppConfig.SaveAsync().ConfigureAwait(false);
        await base.OnDestroyAsync().ConfigureAwait(false);
    }

    [RelayCommand]
    private void ShowAboutPage()
    {
        using AboutViewModel vm = new(base.NavigationService, base.Toolbar, _localizer);
        base.NavigationService.NavigateToAsync(vm);
    }
}
