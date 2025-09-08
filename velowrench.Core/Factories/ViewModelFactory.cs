using velowrench.Calculations.Calculators.Transmission.Chain;
using velowrench.Calculations.Calculators.Transmission.Gear;
using velowrench.Calculations.Calculators.Wheels.SpokeLength;
using velowrench.Calculations.Interfaces;
using velowrench.Core.Enums;
using velowrench.Core.Interfaces;
using velowrench.Core.ViewModels.Help;
using velowrench.Core.ViewModels.Home;
using velowrench.Core.ViewModels.Tools;
using velowrench.Repository.Interfaces;
using velowrench.Utils.Interfaces;

namespace velowrench.Core.Factories;

/// <summary>
/// Factory implementation for creating view model instances for different parts of the application.
/// </summary>
public sealed class ViewModelFactory : IViewModelFactory
{
    private readonly ILocalizer _localizer;
    private readonly ICalculatorFactory<ChainLengthCalculatorInput, ChainLengthCalculatorResult> _chainLengthCalculatorFactory;
    private readonly ICalculatorFactory<GearCalculatorInput, GearCalculatorResult> _gearCalculatorFactory;
    private readonly ICalculatorFactory<SpokeLengthCalculatorInput, SpokeLengthCalculatorResult> _spokeLengthCalculatorFactory;
    private readonly IUnitStore _unitStore;
    private readonly IComponentStandardRepository _componentStandardRepository;
    private readonly IDebounceActionFactory _debounceActionFactory;
    private readonly IToolbar _toolbar;
    private readonly IClipboardInterop _clipboard;
    private readonly IAppConfiguration _appConfiguration;

    public ViewModelFactory(ILocalizer localizer,
        ICalculatorFactory<ChainLengthCalculatorInput, ChainLengthCalculatorResult> chainLengthCalculatorFactory,
        ICalculatorFactory<GearCalculatorInput, GearCalculatorResult> gearCalculatorFactory,
        ICalculatorFactory<SpokeLengthCalculatorInput, SpokeLengthCalculatorResult> spokeLengthCalculatorFactory,
        IUnitStore unitStore,
        IDebounceActionFactory debounceActionFactory,
        IComponentStandardRepository componentStandardRepository,
        IToolbar toolbar,
        IClipboardInterop clipboard,
        IAppConfiguration appConfiguration)
    {

        _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        _chainLengthCalculatorFactory = chainLengthCalculatorFactory ?? throw new ArgumentNullException(nameof(chainLengthCalculatorFactory));
        _gearCalculatorFactory = gearCalculatorFactory ?? throw new ArgumentNullException(nameof(gearCalculatorFactory));
        _spokeLengthCalculatorFactory = spokeLengthCalculatorFactory ?? throw new ArgumentNullException(nameof(spokeLengthCalculatorFactory));
        _unitStore = unitStore ?? throw new ArgumentNullException(nameof(unitStore));
        _debounceActionFactory = debounceActionFactory ?? throw new ArgumentNullException(nameof(debounceActionFactory));
        _componentStandardRepository = componentStandardRepository ?? throw new ArgumentNullException(nameof(componentStandardRepository));
        _toolbar = toolbar ?? throw new ArgumentNullException(nameof(toolbar));
        _clipboard = clipboard ?? throw new ArgumentNullException(nameof(clipboard));
        _appConfiguration = appConfiguration ?? throw new ArgumentNullException(nameof(appConfiguration));
    }

    /// <inheritdoc/>
    public IRoutableViewModel CreateHomeViewModel(INavigationService navigationService)
    {
        return new HomeViewModel(_localizer, navigationService, _toolbar);
    }

    /// <inheritdoc/>
    public IRoutableViewModel CreateProfileViewModel(INavigationService navigationService)
    {
        return new ProfileViewModel(_appConfiguration, _localizer, _unitStore, navigationService, _toolbar);
    }

    /// <inheritdoc/>
    /// <exception cref="NotSupportedException">Thrown when the specified tool type is not supported.</exception>
    public IRoutableViewModel CreateToolViewModel(EVeloWrenchTools type, INavigationService navigationService)
    {
        switch (type)
        {
            case EVeloWrenchTools.ChainLengthCalculator:
                return new ChainLengthCalculatorViewModel(_chainLengthCalculatorFactory, _unitStore, navigationService, _debounceActionFactory, _localizer, _toolbar, _clipboard);
            case EVeloWrenchTools.GearCalculator:
                return new GearCalculatorViewModel(_gearCalculatorFactory, _unitStore, navigationService, _debounceActionFactory, _componentStandardRepository, _localizer, _toolbar, _clipboard);
            case EVeloWrenchTools.SpokeLengthCalculator:
                return new SpokeLengthCalculatorViewModel(_spokeLengthCalculatorFactory, _unitStore, navigationService, _debounceActionFactory, _componentStandardRepository, _localizer, _toolbar, _clipboard);
            default:
                throw new NotSupportedException(type.ToString());
        }
    }

    /// <inheritdoc/>
    /// <exception cref="NotSupportedException">Thrown when the specified tool type does not have help documentation.</exception>
    public IRoutableViewModel CreateHelpViewModel(EVeloWrenchTools type, INavigationService navigationService)
    {
        switch (type)
        {
            case EVeloWrenchTools.ChainLengthCalculator:
                return new ChainLengthCalculatorHelpViewModel(navigationService, _localizer, _toolbar);
            case EVeloWrenchTools.GearCalculator:
                return new GearCalculatorHelpViewModel(navigationService, _localizer, _toolbar);
            case EVeloWrenchTools.SpokeLengthCalculator:
                return new SpokeLengthCalculatorHelpViewModel(navigationService, _localizer, _toolbar);
            default:
                throw new NotSupportedException(type.ToString());
        }
    }
}