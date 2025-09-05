using CommunityToolkit.Mvvm.ComponentModel;
using velowrench.Core.Interfaces;
using velowrench.Core.ViewModels.Base;

namespace velowrench.Core.ViewModels.Help;

public sealed partial class GearCalculatorHelpViewModel : BaseRoutableViewModel
{
    /// <inheritdoc/>
    public override string Name { get; }

    [ObservableProperty]
    private string _gearInchesFormula = @"GI = \frac{F}{R} \cdot D_w";

    [ObservableProperty]
    private string _gainRatioFormula = @"GR = \frac{F}{R} \cdot \frac{L_w}{L_c}";

    [ObservableProperty]
    private string _developmentFormula = @"Dev = \frac{F}{R} \cdot C_w";

    [ObservableProperty]
    private string _speedFormula = @"V = RPM \cdot Dev";

    public GearCalculatorHelpViewModel(
        INavigationService navigationService,
        ILocalizer localizer,
        IToolbar toolbar)
    : base(navigationService, toolbar)
    {
        ArgumentNullException.ThrowIfNull(localizer, nameof(localizer));

        this.Name = localizer.GetString("GearCalculator");
    }
}

