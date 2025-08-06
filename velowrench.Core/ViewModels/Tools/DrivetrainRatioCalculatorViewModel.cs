using velowrench.Core.Interfaces;
using velowrench.Core.ViewModels.Base;

namespace velowrench.Core.ViewModels.Tools;

public partial class DrivetrainRatioCalculatorViewModel : BaseRoutableViewModel
{
    public override string Name { get; }

    public DrivetrainRatioCalculatorViewModel(INavigationService navigationService, ILocalizer localizer) : base(navigationService)
    {
        Name = localizer.GetString("DrivetrainRatioCalculator");
    }
}