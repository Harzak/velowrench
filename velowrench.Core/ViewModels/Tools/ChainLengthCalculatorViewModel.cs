using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;
using UnitsNet.Units;
using velowrench.Calculations.Calculs.Transmission.ChainLength;
using velowrench.Calculations.Interfaces;
using velowrench.Core.Interfaces;
using velowrench.Core.Units;
using velowrench.Core.ViewModels.Base;
using velowrench.Utils.Enums;
using velowrench.Utils.EventArg;
using velowrench.Utils.Results;

namespace velowrench.Core.ViewModels.Tools;

/// <summary>
/// View model for the chain length calculator tool that determines optimal bicycle chain length.
/// </summary>
public sealed partial class ChainLengthCalculatorViewModel : BaseRoutableViewModel
{
    private const int PROGRESS_INDICATOR_DELAY = 350;
    private readonly ICalcul<ChainLengthCalculInput, ChainLengthCalculResult> _calcul;
    private OperationResult<ChainLengthCalculResult>? _lastResult;

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

    /// <summary>
    /// Gets or sets the current state of the chain length calculation.
    /// </summary>
    [ObservableProperty]
    private ECalculState _currentState;

    public ChainLengthCalculatorViewModel(ICalculFactory<ChainLengthCalculInput, ChainLengthCalculResult> calculFactory,
        INavigationService navigationService,
        ILocalizer localizer)
    : base(navigationService)
    {
        ArgumentNullException.ThrowIfNull(calculFactory, nameof(calculFactory));
        ArgumentNullException.ThrowIfNull(navigationService, nameof(navigationService));
        ArgumentNullException.ThrowIfNull(localizer, nameof(localizer));

        _calcul = calculFactory.CreateCalcul();
        _calcul.StateChanged += OnChainLengthCalculStateChanged;
        _chainStayLength = new ConvertibleDouble<LengthUnit>(0, LengthUnit.Centimeter);
        _chainStayLength.ValueChanged += this.OnChainStayLengthValueChanged;

        this.Name = localizer.GetString("ChainLengthCalculator");
        this.CurrentState = ECalculState.NotStarted; ;
    }

    /// <summary>
    /// Handles changes to the chainstay length value and triggers calculation updates.
    /// </summary>
    private void OnChainStayLengthValueChanged(object? sender, EventArgs e)
    {
        this.OnInputsChanged();
    }

    /// <summary>
    /// Partial method called when the largest chainring teeth count changes.
    /// </summary>
    partial void OnTeethLargestChainringChanged(int? value)
    {
        this.OnInputsChanged();
    }

    /// <summary>
    /// Partial method called when the largest sprocket teeth count changes.
    /// </summary>
    partial void OnTeethLargestSprocketChanged(int? value)
    {
        this.OnInputsChanged();
    }

    /// <summary>
    /// Handles input changes and triggers calculation if inputs are valid.
    /// </summary>
    private void OnInputsChanged()
    {
        if (this.CanStartCalculation())
        {
            this.StartCalculation();
        }
        else if (!this.InputsAreValid())
        {
            this.CurrentState = ECalculState.NotStarted;
        }
    }

    /// <summary>
    /// Determines whether a calculation can be started based on current input validity and calculation state.
    /// </summary>
    private bool CanStartCalculation()
    {
        return !base.HasErrors && this.InputsAreValid() && _calcul.State != ECalculState.InProgress;
    }

    /// <summary>
    /// Validates that all required inputs have valid values.
    /// </summary>
    private bool InputsAreValid()
    {
        return _chainStayLength?.Value > 0
                && _teethLargestChainring.HasValue  && _teethLargestChainring > 0
                && _teethLargestSprocket.HasValue && _teethLargestSprocket > 0;
    }

    /// <summary>
    /// Handles state changes from the calculation engine and manages progress indicators.
    /// </summary>
    private async void OnChainLengthCalculStateChanged(object? sender, CalculStateEventArgs e)
    {
        if (this.CurrentState == ECalculState.InProgress && e.State == ECalculState.Computed)
        {
            await Task.Delay(PROGRESS_INDICATOR_DELAY).ConfigureAwait(false);
        }
        this.CurrentState = e.State;
    }

    /// <summary>
    /// Starts the chain length calculation with the current input values.
    /// </summary>
    private void StartCalculation()
    {
        if (base.HasErrors || _calcul.State == ECalculState.InProgress)
        {
            return;
        }

        ChainLengthCalculInput input = new()
        {
            ChainStayLengthInch = this.ChainStayLength?.GetValueIn(LengthUnit.Inch) ?? 0,
            TeethLargestChainring = this.TeethLargestChainring ?? 0,
            TeethLargestSprocket = this.TeethLargestSprocket ?? 0
        };

        if (_lastResult?.Content?.UsedInputs != null && _lastResult.Content.UsedInputs == input)
        {
            return;
        }

        _lastResult = _calcul.Start(input);

        if (_lastResult.IsSuccess && _lastResult.HasContent)
        {
            this.RecommendedChainLength = new ConvertibleDouble<LengthUnit>(_lastResult.Content.ChainLengthInch, LengthUnit.Inch);
            this.RecommendedChainLinks = _lastResult.Content.ChainLinks;
        }
    }

    /// <summary>
    /// Shows the help page for the chain length calculator.
    /// </summary>
    public override void ShowHelpPage()
    {
        base.NavigationService.NavigateToHelp(Enums.EVeloWrenchTools.ChainLengthCalculator);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (_calcul != null)
            {
                _calcul.StateChanged -= OnChainLengthCalculStateChanged;
            }
            if (_chainStayLength != null)
            {
                _chainStayLength.ValueChanged -= this.OnChainStayLengthValueChanged;
            }
        }
        base.Dispose(disposing);
    }
}