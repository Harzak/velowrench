using velowrench.Core.Enums;
using velowrench.Core.Interfaces;
using velowrench.Core.Services;
using velowrench.Core.ViewModels.Home;
using velowrench.Core.ViewModels.Tools;

namespace velowrench.Core.Factories;

public class ViewModelFactory : IViewModelFactory
{
    private readonly ILocalizer _localizer;

    public ViewModelFactory(ILocalizer localizer)
    {
        _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
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
                return new ChainLengthCalculatorViewModel(navigationService, _localizer);
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

}