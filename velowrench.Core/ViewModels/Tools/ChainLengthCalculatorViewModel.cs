using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;
using UnitsNet.Units;
using velowrench.Calculations.Calculators.Transmission.Chain;
using velowrench.Calculations.Interfaces;
using velowrench.Calculations.Units;
using velowrench.Core.Interfaces;
using velowrench.Core.ViewModels.Base;
using velowrench.Utils.Enums;
using velowrench.Utils.EventArg;
using velowrench.Utils.Results;

namespace velowrench.Core.ViewModels.Tools;

/// <summary>
/// View model for the chain length calculator tool that determines optimal bicycle chain length.
/// </summary>
public sealed partial class ChainLengthCalculatorViewModel : BaseCalculatorViewModel<ChainLengthCalculatorInput, ChainLengthCalculatorResult>
{
    /// <summary>
    /// Gets the display name of this view model.
    /// </summary>
    public override string Name { get; }

    /// <summary>
    /// Gets a value indicating whether this view model has an associated help page.
    /// </summary>
    public override bool CanShowHelpPage => true;

    /// <summary>
    /// Gets or sets the chainstay length measurement with unit conversion capabilities.
    /// </summary>
    [ObservableProperty]
    private ConvertibleDouble<LengthUnit>? _chainStayLength;

    /// <summary>
    /// Gets or sets the number of teeth on the largest chainring (front gear).
    /// </summary>
    [ObservableProperty]
    [Range(1, 100, ErrorMessage = "Must be between 1 and 100 teeth")]
    [NotifyDataErrorInfo]
    private int? _teethLargestChainring;

    /// <summary>
    /// Gets or sets the number of teeth on the largest sprocket (rear gear).
    /// </summary>
    [ObservableProperty]
    [Range(1, 70, ErrorMessage = "Must be between 1 and 70 teeth")]
    [NotifyDataErrorInfo]
    private int? _teethLargestSprocket;

    /// <summary>
    /// Gets or sets the recommended number of chain links for the specified drivetrain configuration.
    /// </summary>
    [ObservableProperty]
    private int _recommendedChainLinks;

    /// <summary>
    /// Gets or sets the calculated chain length with unit conversion capabilities.
    /// </summary>
    [ObservableProperty]
    private ConvertibleDouble<LengthUnit>? _recommendedChainLength;

    public ChainLengthCalculatorViewModel(
        ICalculatorFactory<ChainLengthCalculatorInput, ChainLengthCalculatorResult> calculatorFactory,
        INavigationService navigationService,
        IDebounceActionFactory actionFactory,
        ILocalizer localizer)
    : base(calculatorFactory, navigationService, actionFactory)
    {
        ArgumentNullException.ThrowIfNull(localizer, nameof(localizer));

        _chainStayLength = new ConvertibleDouble<LengthUnit>(0, LengthUnit.Centimeter);
        _chainStayLength.ValueChanged += this.OnChainStayLengthValueChanged;

        this.Name = localizer.GetString("ChainLengthCalculator");
    }

    /// <summary>
    /// Handles changes to the chainstay length value and triggers calculation updates.
    /// </summary>
    private void OnChainStayLengthValueChanged(object? sender, EventArgs e)
    {
        base.RefreshCalculationDebounced.Execute();
    }

    /// <summary>
    /// Partial method called when the largest chainring teeth count changes.
    /// </summary>
    partial void OnTeethLargestChainringChanged(int? value)
    {
        base.RefreshCalculationDebounced.Execute();
    }

    /// <summary>
    /// Partial method called when the largest sprocket teeth count changes.
    /// </summary>
    partial void OnTeethLargestSprocketChanged(int? value)
    {
        base.RefreshCalculationDebounced.Execute();
    }

    protected override ChainLengthCalculatorInput GetInput()
    {
        return new ChainLengthCalculatorInput()
        {
            ChainStayLength = this.ChainStayLength ?? ConvertibleDouble<LengthUnit>.Default(),
            TeethLargestChainring = this.TeethLargestChainring ?? 0,
            TeethLargestSprocket = this.TeethLargestSprocket ?? 0
        };
    }

    protected override void OnCalculationSuccessful(OperationResult<ChainLengthCalculatorResult> result)
    {
        this.RecommendedChainLength = result.Content.ChainLength;
        this.RecommendedChainLinks = result.Content.ChainLinks;
    }

    /// <summary>
    /// Shows the help page for the chain length calculator.
    /// </summary>
    public override void ShowHelpPage()
    {
        base.NavigationService.NavigateToHelp(Enums.EVeloWrenchTools.ChainLengthCalculator);
    }
}