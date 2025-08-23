using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Core.Interfaces;
using velowrench.Core.ViewModels.Base;

namespace velowrench.Core.ViewModels.Help;
public sealed partial class SpokeLengthCalculatorRimMeasurementHelpViewModel : BaseRoutableViewModel
{
    public override string Name { get; }

    public SpokeLengthCalculatorRimMeasurementHelpViewModel(INavigationService navigationService, ILocalizer localizer) : base(navigationService)
    {
        ArgumentNullException.ThrowIfNull(localizer, nameof(localizer));

        this.Name = localizer.GetString("RimMeasurements");
    }
}