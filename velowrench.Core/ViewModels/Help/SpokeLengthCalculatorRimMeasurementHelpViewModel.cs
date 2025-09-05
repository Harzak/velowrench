using velowrench.Core.Interfaces;
using velowrench.Core.ViewModels.Base;

namespace velowrench.Core.ViewModels.Help;
public sealed partial class SpokeLengthCalculatorRimMeasurementHelpViewModel : BaseRoutableViewModel
{
    /// <inheritdoc/>
    public override string Name { get; }

    public SpokeLengthCalculatorRimMeasurementHelpViewModel(
        INavigationService navigationService,
        ILocalizer localizer,
        IToolbar toolbar)
    : base(navigationService, toolbar)
    {
        ArgumentNullException.ThrowIfNull(localizer, nameof(localizer));

        this.Name = localizer.GetString("RimMeasurements");
    }
}