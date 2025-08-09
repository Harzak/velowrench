using velowrench.Core.Interfaces;
using velowrench.Core.ViewModels.Base;

namespace velowrench.Core.ViewModels.Tools;

public sealed partial class RolloutCalculatorViewModel : BaseRoutableViewModel
{
    public override string Name { get; }

    public RolloutCalculatorViewModel(INavigationService navigationService, ILocalizer localizer) : base(navigationService)
    {
        Name = localizer.GetString("RolloutCalculator");
    }
}