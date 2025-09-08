using velowrench.Core.Interfaces;
using velowrench.Core.ViewModels.Base;

namespace velowrench.Core.ViewModels.Home;

public sealed partial class AboutViewModel : BaseRoutableViewModel
{
    public override string Name { get; }

    public AboutViewModel(
        INavigationService navigationService,
        IToolbar toolbar,
        ILocalizer localizer)
    : base(navigationService, toolbar)
    {
        ArgumentNullException.ThrowIfNull(localizer, nameof(localizer));

        Name = localizer.GetString("About");
    }
}
