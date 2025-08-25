using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using velowrench.Calculations.Calculators;
using velowrench.Calculations.Interfaces;
using velowrench.Core.Interfaces;
using velowrench.Utils.Enums;
using velowrench.Utils.EventArg;
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

    private OperationResult<TResult>? _lastResult;
    private readonly Dictionary<string, string> _inputErrors;

    /// <summary>
    /// Schedules a calculation to run after a brief delay, canceling any pending calculations.
    /// This prevents excessive calculations during rapid UI changes.
    /// </summary>
    private IDebounceAction _refreshCalculationDebounced { get; }

    /// <summary>
    /// Gets the calculation engine instance used to perform calculations.
    /// This instance handles the actual calculation logic, validation, and state management.
    /// </summary>
    protected ICalculator<TInput, TResult> Calculator { get; }

    /// <summary>
    /// Gets the input required by the calculator to perform its operations.
    /// </summary>
    protected abstract TInput CalculatorInput { get; }

    /// <summary>
    /// Gets or sets the current state of the calculation operation.
    /// </summary>
    [ObservableProperty]
    private ECalculatorState _currentState;

    /// <summary>
    /// Gets error messages from input validation failures.
    /// </summary>
    public IEnumerable<string> InputErrorMessages => _inputErrors.Values.ToList();

    protected BaseCalculatorViewModel(
        ICalculatorFactory<TInput, TResult> calculatorFactory,
        INavigationService navigationService,
        IDebounceActionFactory actionFactory) : base(navigationService)
    {
        ArgumentNullException.ThrowIfNull(actionFactory, nameof(actionFactory));

        _inputErrors = [];

        this.Calculator = calculatorFactory?.CreateCalculator() ?? throw new ArgumentNullException(nameof(calculatorFactory));
        this.Calculator.StateChanged += OnCalculatorStateChanged;
        this.CurrentState = ECalculatorState.NotStarted;

        this._refreshCalculationDebounced = actionFactory.CreateDebounceUIAction(RefreshCalculation);
    }

    /// <summary>
    /// Handles changes to a calculation input property and updates the calculation state.
    /// </summary>
    protected void OnCalculationInputChanged(string propertyName)
    {
        ICalculatorInputValidation<TInput> validation = this.Calculator.GetValidation();
        bool isValid = validation.Validate(CalculatorInput);

        if (_inputErrors.ContainsKey(propertyName) && !validation.ErrorMessages.ContainsKey(propertyName))
        {
            _inputErrors.Remove(propertyName);
            base.OnPropertyChanged(nameof(this.InputErrorMessages));
        }

        if (isValid)
        {
            this._refreshCalculationDebounced.Execute();
            return;
        }

        this.CurrentState = ECalculatorState.NotStarted;

        if (validation.ErrorMessages.TryGetValue(propertyName, out string? value) && !_inputErrors.ContainsKey(propertyName))
        {
            _inputErrors[propertyName] = value;
            base.OnPropertyChanged(nameof(this.InputErrorMessages));
        }
    }

    private void RefreshCalculation()
    {
        if (this.CanCalculate())
        {
            this.Calculate();
        }
    }

    protected virtual bool CanCalculate()
    {
        return !base.HasErrors
            && this.Calculator.GetValidation().Validate(CalculatorInput)
            && this.Calculator.State != ECalculatorState.InProgress
            && (_lastResult == null || !_lastResult.Content.UsedInputs.Equals(CalculatorInput));
    }

    private void Calculate()
    {
        _lastResult = this.Calculator.Start(CalculatorInput);

        if (_lastResult.IsSuccess && _lastResult.HasContent)
        {
            this.OnCalculationSuccessful(_lastResult);
        }
    }

    private async void OnCalculatorStateChanged(object? sender, CalculatorStateEventArgs e)
    {
        if (this.CurrentState == ECalculatorState.InProgress && e.State == ECalculatorState.Computed)
        {
            await Task.Delay(PROGRESS_INDICATOR_DELAY).ConfigureAwait(false);
        }
        this.CurrentState = e.State;
    }

    protected abstract void OnCalculationSuccessful(OperationResult<TResult> result);

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (this.Calculator != null)
            {
                this.Calculator.StateChanged -= OnCalculatorStateChanged;
            }
            _refreshCalculationDebounced?.Dispose();
        }
        base.Dispose(disposing);
    }
}
