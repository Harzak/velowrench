using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using velowrench.Core.Interfaces;
using velowrench.Core.ViewModels.Base;

namespace velowrench.Core.ViewModels.Help;

public sealed partial class SpokeLengthCalculatorHelpViewModel : BaseRoutableViewModel
{
    private readonly ILocalizer _localizer;

    /// <summary>
    /// Gets the display name of the help view model.
    /// </summary>
    public override string Name { get; }

    [ObservableProperty]
    private string _definitionLaTeX = @"R = \frac{ERD}{2}, \; r_{s} = \frac{D_{s}}{2}, \; \theta = \frac{2 \pi X}{S / 2}, \; s \in \{L, R\}";

    [ObservableProperty]
    private string _baselineChordCLaTeX = @"C_{s} = \sqrt{ R^{2} + r_{s}^{2} - 2 R r_{s} \cos(\theta) }";

    [ObservableProperty]
    private string _spokeLengthLeftLaTeX = @"L_{L} = \sqrt{ C_{L}^{2} + \left(w_{L}\right)^{2} } - \frac{d}{2}";

    [ObservableProperty]
    private string _spokeLengthRightLaTeX = @"L_{R} = \sqrt{ C_{R}^{2} + \left(w_{R}\right)^{2} } - \frac{d}{2}";

    public SpokeLengthCalculatorHelpViewModel(
        INavigationService navigationService,
        ILocalizer localizer,
        IToolbar toolbar)
    : base(navigationService, toolbar)
    {
        _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));

        this.Name = localizer.GetString("SpokeLengthCalculator");
    }

    [RelayCommand]
    private void ShowHubMeasurementsHelpPage()
    {
        using SpokeLengthCalculatorHubMeasurementHelpViewModel vm = new(NavigationService, _localizer, base.Toolbar);
        base.NavigationService.NavigateToAsync(vm);
    }

    [RelayCommand]
    private void ShowRimMeasurementsHelpPage()
    {
        using SpokeLengthCalculatorRimMeasurementHelpViewModel vm = new(NavigationService, _localizer, base.Toolbar);
        base.NavigationService.NavigateToAsync(vm);
    }

    [RelayCommand]
    private void ShowBuildConfigurationHelpPage()
    {
        using SpokeLengthCalculatorBuildConfigurationHelpViewModel vm = new(NavigationService, _localizer, base.Toolbar);
        base.NavigationService.NavigateToAsync(vm);
    }
}
