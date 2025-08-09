using velowrench.Core.Interfaces;
using velowrench.Core.ViewModels.Base;

namespace velowrench.Core.ViewModels.Tools;

public sealed partial class RolloutCalculatorViewModel : BaseRoutableViewModel
{
    public override string Name { get; }

    public RolloutCalculatorViewModel(INavigationService navigationService, ILocalizer localizer) : base(navigationService)
    {
        ArgumentNullException.ThrowIfNull(navigationService, nameof(navigationService));
        ArgumentNullException.ThrowIfNull(localizer, nameof(localizer));

        Name = localizer.GetString("RolloutCalculator");
    }
}