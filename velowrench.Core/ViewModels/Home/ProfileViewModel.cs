using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Core.Interfaces;
using velowrench.Core.ViewModels.Base;

namespace velowrench.Core.ViewModels.Home;

public sealed partial class ProfileViewModel : BaseRoutableViewModel
{
    /// <summary>
    /// Gets the display name of the home view model.
    /// </summary>
    public override string Name { get; }

    public ProfileViewModel(
        ILocalizer localizer,
        INavigationService navigationService,
        IToolbar toolbar)
    : base(navigationService, toolbar)
    {
        ArgumentNullException.ThrowIfNull(localizer, nameof(localizer));
        ArgumentNullException.ThrowIfNull(navigationService, nameof(navigationService));

        Name = localizer.GetString("ProfileKey");
    }
}
