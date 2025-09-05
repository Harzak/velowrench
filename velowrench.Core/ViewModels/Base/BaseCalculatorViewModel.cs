using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using velowrench.Calculations.Calculators;
using velowrench.Calculations.Interfaces;
using velowrench.Calculations.Validation.Results;
using velowrench.Core.Interfaces;
using velowrench.Core.Validation;
using velowrench.Utils.Enums;
using velowrench.Utils.EventArg;
using velowrench.Utils.Interfaces;
using velowrench.Utils.Results;

namespace velowrench.Core.ViewModels.Base;

/// <summary>
/// Provides a base implementation for view models that perform calculations with input validation,
/// state management, and result handling. This abstract class standardizes the calculation workflow
/// across different calculation types while allowing specific implementations for input creation and result processing.
/// </summary>
public abstract partial class BaseCalculatorViewModel<TInput, TResult> : BaseRoutableViewModel
    where TInput : class
    where TResult : BaseCalculatorResult<TInput>
{
    private const int PROGRESS_INDICATOR_DELAY = 350;

    private readonly IDebounceAction _refreshCalculationDebounced;
    private OperationResult<TResult>? _lastResult;
    private readonly List<ValidationError> _inputErrors;
    private bool _isProgrammaticChange;

    /// <summary>
    /// Gets the calculation engine instance used to perform calculations.
    /// This instance handles the actual calculation logic, validation, and state management.
    /// </summary>
    protected ICalculator<TInput, TResult> Calculator { get; }

    /// <summary>
    /// Gets the input required by the calculator to perform its operations.
    /// </summary>
    protected abstract TInput CalculatorInput { get; set; }

    /// <summary>
    /// Gets the clipboard interop service used for interacting with the system clipboard.
    /// </summary>
    protected IClipboardInterop Clipboard { get; }

    protected ILocalizer Localizer { get; }

    /// <summary>
    /// Gets or sets the current state of the calculation operation.
    /// </summary>
    [ObservableProperty]
    private ECalculatorState _currentState;

    /// <summary>
    /// Gets error messages from input validation failures.
    /// </summary>
    public IEnumerable<string> InputErrorMessages => _inputErrors.Select(e => e.Message);

    /// <summary>
    /// Gets the unit store used to manage and retrieve units of measurements.
    /// </summary>
    public IUnitStore UnitStore { get; }

    protected BaseCalculatorViewModel(
        ICalculatorFactory<TInput, TResult> calculatorFactory,
        IUnitStore unitStore,
        INavigationService navigationService,
        IDebounceActionFactory actionFactory,
        ILocalizer localizer,
        IToolbar toolbar,
        IClipboardInterop clipboard) : base(navigationService, toolbar)
    {
        ArgumentNullException.ThrowIfNull(actionFactory, nameof(actionFactory));

        _inputErrors = [];
        _refreshCalculationDebounced = actionFactory.CreateDebounceUIAction(RefreshCalculation);

        this.Calculator = calculatorFactory?.CreateCalculator() ?? throw new ArgumentNullException(nameof(calculatorFactory));
        this.UnitStore = unitStore ?? throw new ArgumentNullException(nameof(unitStore));
        this.Localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        this.Clipboard = clipboard ?? throw new ArgumentNullException(nameof(clipboard));
        this.Calculator.StateChanged += OnCalculatorStateChanged;
        this.CurrentState = ECalculatorState.NotStarted;
    }

    /// <inheritdoc/>
    public override Task OnInitializedAsync()
    {
        base.Toolbar.ResetToDefaultAction = this.ResetToDefault;
        return base.OnInitializedAsync();
    }

    /// <summary>
    /// Handles changes to a calculation input property and updates the calculation state.
    /// This method now integrates with the enhanced validation system.
    /// </summary>
    protected void OnCalculationInputChanged(string propertyName)
    {
        if (_isProgrammaticChange)
        {
            return;
        }

        ValidationResult result = this.Calculator.InputValidator.ValidateProperty(CalculatorInput, propertyName, new ValidationContext(EValidationMode.Progressive));
        ValidationError? previousError = _inputErrors.FirstOrDefault(e => e.PropertyName == propertyName);
        if (previousError != null)
        {
            _inputErrors.Remove(previousError);
        }

        ValidationError? currentError = result.Errors.FirstOrDefault(e => e.PropertyName == propertyName);

        if (currentError != null)
        {
            _inputErrors.Add(currentError);
        }

        base.OnPropertyChanged(nameof(this.InputErrorMessages));

        if (result.IsValid)
        {
            this._refreshCalculationDebounced.Execute();
        }
        else
        {
            this.CurrentState = ECalculatorState.NotStarted;
        }
    }

    private void RefreshCalculation()
    {
        if (this.CanCalculate())
        {
            this.Calculate();
        }
    }

    private void Calculate()
    {
        _lastResult = this.Calculator.Start(CalculatorInput);

        if (_lastResult.IsSuccess && _lastResult.HasContent)
        {
            this.OnCalculationSuccessful(_lastResult);
        }
    }

    protected virtual bool CanCalculate()
    {
        return !base.HasErrors
            && IsInputValid()
            && this.Calculator.State != ECalculatorState.InProgress
            && (_lastResult?.Content?.UsedInputs == null || !_lastResult.Content.UsedInputs.Equals(CalculatorInput));
    }

    protected abstract void OnCalculationSuccessful(OperationResult<TResult> result);

    private async void OnCalculatorStateChanged(object? sender, CalculatorStateEventArgs e)
    {
        if (this.CurrentState == ECalculatorState.InProgress && e.State == ECalculatorState.Computed)
        {
            await Task.Delay(PROGRESS_INDICATOR_DELAY).ConfigureAwait(false);
        }
        this.CurrentState = e.State;
    }

    private bool IsInputValid()
    {
        if (_inputErrors.Count > 0)
        {
            return false;
        }
        ValidationResult result = this.Calculator.InputValidator.ValidateWithResults(CalculatorInput);
        return result.IsValid;
    }

    public virtual void ResetToDefault()
    {
        _lastResult = null;
        _inputErrors.Clear();
        this.CurrentState = ECalculatorState.NotStarted;
    }

    protected void WithProgrammaticChange(Action? action)
    {
        _isProgrammaticChange = true;
        action?.Invoke();
        _isProgrammaticChange = false;
    }

    private bool _disposed;
    protected override void Dispose(bool disposing)
    {
        if (disposing && !_disposed)
        {
            if (this.Calculator != null)
            {
                this.Calculator.StateChanged -= OnCalculatorStateChanged;
            }
            _refreshCalculationDebounced?.Dispose();
            _disposed = true;
        }
        base.Dispose(disposing);
    }
}
