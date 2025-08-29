using Avalonia.Platform;
using velowrench.Calculations.Calculators.Transmission.Chain;
using velowrench.Calculations.Calculators.Transmission.Gear;
using velowrench.Calculations.Calculators.Wheels.SpokeLength;
using velowrench.Calculations.Interfaces;
using velowrench.Core.Enums;
using velowrench.Core.Interfaces;
using velowrench.Core.Services;
using velowrench.Core.ViewModels.Help;
using velowrench.Core.ViewModels.Home;
using velowrench.Core.ViewModels.Tools;
using velowrench.Repository.Interfaces;

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
    private readonly IComponentStandardRepository _componentStandardRepository;
    private readonly IDebounceActionFactory _debounceActionFactory;

    public ViewModelFactory(ILocalizer localizer,
        ICalculatorFactory<ChainLengthCalculatorInput, ChainLengthCalculatorResult> chainLengthCalculatorFactory,
        ICalculatorFactory<GearCalculatorInput, GearCalculatorResult> gearCalculatorFactory,
        ICalculatorFactory<SpokeLengthCalculatorInput, SpokeLengthCalculatorResult> spokeLengthCalculatorFactory,
        IDebounceActionFactory debounceActionFactory,
        IComponentStandardRepository componentStandardRepository)
    {

        _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        _chainLengthCalculatorFactory = chainLengthCalculatorFactory ?? throw new ArgumentNullException(nameof(chainLengthCalculatorFactory));
        _gearCalculatorFactory = gearCalculatorFactory ?? throw new ArgumentNullException(nameof(gearCalculatorFactory));
        _spokeLengthCalculatorFactory = spokeLengthCalculatorFactory ?? throw new ArgumentNullException(nameof(spokeLengthCalculatorFactory));
        _debounceActionFactory = debounceActionFactory ?? throw new ArgumentNullException(nameof(debounceActionFactory));
        _componentStandardRepository = componentStandardRepository ?? throw new ArgumentNullException(nameof(componentStandardRepository));
    }

    /// <summary>
    /// Creates the home page view model.
    /// </summary>
    /// <returns>A routable view model instance for the home page.</returns>
    public IRoutableViewModel CreateHomeViewModel(INavigationService navigationService)
    {
        return new HomeViewModel(_localizer, navigationService);
    }

    /// <summary>
    /// Creates a tool-specific view model for the specified tool type.
    /// </summary>
    /// <returns>A routable view model instance for the specified tool.</returns>
    /// <exception cref="NotSupportedException">Thrown when the specified tool type is not supported.</exception>
    public IRoutableViewModel CreateToolViewModel(EVeloWrenchTools type, INavigationService navigationService)
    {
        switch (type)
        {
            case EVeloWrenchTools.ChainLengthCalculator:
                return new ChainLengthCalculatorViewModel(_chainLengthCalculatorFactory, navigationService, _debounceActionFactory, _localizer);
            case EVeloWrenchTools.GearCalculator:
                return new GearCalculatorViewModel(_gearCalculatorFactory, navigationService, _debounceActionFactory, _componentStandardRepository, _localizer);
            case EVeloWrenchTools.SpokeLengthCalculator:
                return new SpokeLengthCalculatorViewModel(_spokeLengthCalculatorFactory, navigationService, _debounceActionFactory, _componentStandardRepository, _localizer);
            default:
                throw new NotSupportedException(type.ToString());
        }
    }

    /// <summary>
    /// Creates a help view model for the specified tool type.
    /// </summary>
    /// <returns>A routable view model instance for the tool's help page.</returns>
    /// <exception cref="NotSupportedException">Thrown when the specified tool type does not have help documentation.</exception>
    public IRoutableViewModel CreateHelpViewModel(EVeloWrenchTools type, INavigationService navigationService)
    {
        switch (type)
        {
            case EVeloWrenchTools.ChainLengthCalculator:
                return new ChainLengthCalculatorHelpViewModel(navigationService, _localizer);
            case EVeloWrenchTools.GearCalculator:
                return new GearCalculatorHelpViewModel(navigationService, _localizer);
            case EVeloWrenchTools.SpokeLengthCalculator:
                return new SpokeLengthCalculatorHelpViewModel(navigationService,  _localizer );
            default:
                throw new NotSupportedException(type.ToString());
        }
    }

}