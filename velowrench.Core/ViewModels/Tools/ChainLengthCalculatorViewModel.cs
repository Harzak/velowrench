using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using UnitsNet.Units;
using velowrench.Calculations.Calculators.Transmission.Chain;
using velowrench.Calculations.Interfaces;
using velowrench.Core.Interfaces;
using velowrench.Core.Units;
using velowrench.Core.ViewModels.Base;
using velowrench.Utils.Interfaces;
using velowrench.Utils.Results;

namespace velowrench.Core.ViewModels.Tools;

/// <summary>
/// View model for the chain length calculator tool that determines optimal bicycle chain length.
/// </summary>
public sealed partial class ChainLengthCalculatorViewModel : BaseCalculatorViewModel<ChainLengthCalculatorInput, ChainLengthCalculatorResult>
{
    /// <inheritdoc />
    protected override ChainLengthCalculatorInput CalculatorInput { get; set; }

    /// <inheritdoc/>
    public override string Name { get; }

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
        IUnitStore unitStore,
        INavigationService navigationService,
        IDebounceActionFactory actionFactory,
        ILocalizer localizer,
        IToolbar toolbar)
    : base(calculatorFactory, unitStore, navigationService, actionFactory, toolbar)
    {
        ArgumentNullException.ThrowIfNull(localizer, nameof(localizer));

        this.Name = localizer.GetString("ChainLengthCalculator");
        this.CalculatorInput = new ChainLengthCalculatorInput();
    }

    /// <inheritdoc/>
    public override Task OnInitializedAsync()
    {
        base.Toolbar.ShowHelpPageAction = () => base.NavigationService.NavigateToHelpAsync(Enums.EVeloWrenchTools.ChainLengthCalculator);
        this.FillDefaultValues();
        return base.OnInitializedAsync();
    }

    /// Updates default values with arbitrary values until user configuration management is implemented.
    private void FillDefaultValues()
    {
        base.WithProgrammaticChange(() =>
        {
            this.ChainStayLength = new ConvertibleDouble<LengthUnit>(420, LengthUnit.Millimeter, OnChainStayLengthChanged);
            this.ChainStayLength.Unit = base.UnitStore.LengthDefaultUnit;
            this.TeethLargestChainring = null;
            this.TeethLargestSprocket = null;
            this.RecommendedChainLength = null;
        });
    }

    protected override void OnCalculationSuccessful(OperationResult<ChainLengthCalculatorResult> result)
    {
        this.RecommendedChainLength = new ConvertibleDouble<LengthUnit>(result.Content.ChainLengthIn, LengthUnit.Inch);
        this.RecommendedChainLength.Unit = base.UnitStore.LengthDefaultUnit;
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
        this.CalculatorInput.TeethLargestSprocket = value ?? 0;
        base.OnCalculationInputChanged(nameof(CalculatorInput.TeethLargestSprocket));
    }

    public override void ResetToDefault()
    {
        this.CalculatorInput = new ChainLengthCalculatorInput();
        this.FillDefaultValues();
        base.ResetToDefault();
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        if (_disposed)
        {
            return;
        }
        base.OnPropertyChanged(e);
    }

    private bool _disposed;
    protected override void Dispose(bool disposing)
    {
        if (disposing && !_disposed)
        {
            _disposed = true;

            _teethLargestChainring = null;
            _teethLargestSprocket = null;
            _chainStayLength = null;
            _recommendedChainLength = null;
        }
        base.Dispose(disposing);
    }
}