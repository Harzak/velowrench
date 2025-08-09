using velowrench.Calculations.Calculs.Transmission.ChainLength;
using velowrench.Calculations.Interfaces;
using velowrench.Core.Enums;
using velowrench.Core.Interfaces;
using velowrench.Core.ViewModels.Help;
using velowrench.Core.ViewModels.Home;
using velowrench.Core.ViewModels.Tools;

namespace velowrench.Core.Factories;

public class ViewModelFactory : IViewModelFactory
{
    private readonly ILocalizer _localizer;
    private readonly ICalculFactory<ChainLengthCalculInput, ChainLengthCalculResult> _chainLengthCalculFactory;

    public ViewModelFactory(ILocalizer localizer,
        ICalculFactory<ChainLengthCalculInput, ChainLengthCalculResult> calculFactory)
    {
        _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        _chainLengthCalculFactory = calculFactory ?? throw new ArgumentNullException(nameof(calculFactory));
    }

    public IRoutableViewModel CreateHomeViewModel(INavigationService navigationService)
    {
        return new HomeViewModel(_localizer, navigationService);
    }

    public IRoutableViewModel CreateToolViewModel(EVeloWrenchTools type, INavigationService navigationService)
    {
        switch (type)
        {
            case EVeloWrenchTools.ChainLengthCalculator:
                return new ChainLengthCalculatorViewModel(_chainLengthCalculFactory, navigationService, _localizer);
            case EVeloWrenchTools.ChainlineCalculator:
                return new ChainlineCalculatorViewModel(navigationService, _localizer);
            case EVeloWrenchTools.DrivetrainRatioCalculator:
                return new DrivetrainRatioCalculatorViewModel(navigationService, _localizer);
            case EVeloWrenchTools.RolloutCalculator:
                return new RolloutCalculatorViewModel(navigationService, _localizer);
            default:
                throw new NotSupportedException(type.ToString());
        }
    }

    public IRoutableViewModel CreateHelpViewModel(EVeloWrenchTools type, INavigationService navigationService)
    {
        switch (type)
        {
            case EVeloWrenchTools.ChainLengthCalculator:
                return new ChainLengthCalculatorHelpViewModel(navigationService, _localizer);
            default:
                throw new NotSupportedException(type.ToString());
        }
    }

}