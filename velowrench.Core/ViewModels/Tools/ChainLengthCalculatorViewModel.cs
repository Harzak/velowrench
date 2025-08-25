using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;
using UnitsNet.Units;
using velowrench.Calculations.Calculators.Transmission.Chain;
using velowrench.Calculations.Interfaces;
using velowrench.Core.Interfaces;
using velowrench.Core.Units;
using velowrench.Core.ViewModels.Base;
using velowrench.Utils.Results;

namespace velowrench.Core.ViewModels.Tools;

/// <summary>
/// View model for the chain length calculator tool that determines optimal bicycle chain length.
/// </summary>
public sealed partial class ChainLengthCalculatorViewModel : BaseCalculatorViewModel<ChainLengthCalculatorInput, ChainLengthCalculatorResult>
{
    /// <summary>
    /// Gets the input parameters required for the chain length calculation.
    /// </summary>
    protected override ChainLengthCalculatorInput CalculatorInput { get; }

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
    private int? _teethLargestChainring;

    /// <summary>
    /// Gets or sets the number of teeth on the largest sprocket (rear gear).
    /// </summary>
    [ObservableProperty]
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

        this.Name = localizer.GetString("ChainLengthCalculator");
        this.CalculatorInput = new ChainLengthCalculatorInput();

        this.FillDefaultValues();
    }

    /// Updates default values with arbitrary values until user configuration management is implemented.
    private void FillDefaultValues()
    {
        this.ChainStayLength = new ConvertibleDouble<LengthUnit>(420, LengthUnit.Millimeter, OnChainStayLengthChanged);
    }

    protected override void OnCalculationSuccessful(OperationResult<ChainLengthCalculatorResult> result)
    {
        this.RecommendedChainLength = new ConvertibleDouble<LengthUnit>(result.Content.ChainLengthIn, LengthUnit.Inch);
        this.RecommendedChainLinks = result.Content.ChainLinks;
    }

    partial void OnChainStayLengthChanged(ConvertibleDouble<LengthUnit>? value)
    {
        this.CalculatorInput.ChainStayLengthIn = value?.GetValueIn(LengthUnit.Inch) ?? 0;
        base.OnCalculationInputChanged(nameof(CalculatorInput.ChainStayLengthIn));
    }

    partial void OnTeethLargestChainringChanged(int? value)
    {
        this.CalculatorInput.TeethLargestChainring = value ?? 0;
        base.OnCalculationInputChanged(nameof(CalculatorInput.TeethLargestChainring));
    }

    partial void OnTeethLargestSprocketChanged(int? value)
    {
        this.CalculatorInput.TeethLargestSprocket = this.TeethLargestSprocket ?? 0;
        base.OnCalculationInputChanged(nameof(CalculatorInput.TeethLargestSprocket));
    }

    public override void ShowHelpPage()
    {
        base.NavigationService.NavigateToHelp(Enums.EVeloWrenchTools.ChainLengthCalculator);
    }
}