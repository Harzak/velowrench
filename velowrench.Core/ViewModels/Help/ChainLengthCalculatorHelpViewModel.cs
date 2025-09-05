using CommunityToolkit.Mvvm.ComponentModel;
using velowrench.Core.Interfaces;
using velowrench.Core.ViewModels.Base;

namespace velowrench.Core.ViewModels.Help;

/// <summary>
/// View model for the chain length calculator help page that provides detailed calculation information and formulas.
/// </summary>
public sealed partial class ChainLengthCalculatorHelpViewModel : BaseRoutableViewModel
{
    /// <summary>
    /// Gets the display name of the help view model.
    /// </summary>
    public override string Name { get; }

    /// <summary>
    /// Gets or sets the LaTeX representation of the simplified chain length equation.
    /// </summary>
    /// <value>
    [ObservableProperty]
    private string _simpleEquationLaTeX = @"L = 2 \cdot C + \frac{F}{4} + \frac{R}{4} + 1";

    /// <summary>
    /// Gets or sets the LaTeX representation of the rigorous chain length equation.
    /// </summary>
    [ObservableProperty]
    private string _rigorousEquationLaTeX = @"L = 0.25 \cdot \left(F + R\right) + 2 \cdot \sqrt{C^{2} + \left(0.0796 \cdot \left(F - R\right)\right)^{2}}";

    public ChainLengthCalculatorHelpViewModel(
        INavigationService navigationService,
        ILocalizer localizer,
        IToolbar toolbar)
    : base(navigationService, toolbar)
    {
        ArgumentNullException.ThrowIfNull(localizer, nameof(localizer));

        this.Name = localizer.GetString("ChainLengthCalculator");
    }
}
