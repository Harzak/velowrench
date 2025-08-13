using CommunityToolkit.Mvvm.ComponentModel;
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

    /// <summary>
    /// Gets the calculation engine instance used to perform calculations.
    /// This instance handles the actual calculation logic, validation, and state management.
    /// </summary>
    protected ICalculator<TInput, TResult> Calculator { get; }

    /// <summary>
    /// Gets or sets the current state of the calculation operation.
    /// </summary>
    [ObservableProperty]
    private ECalculatorState _currentState;

    /// <summary>
    /// Gets or sets error messages from input validation failures.
    /// </summary>
    [ObservableProperty]
    private string? _calculatorInputErrors;

    protected BaseCalculatorViewModel(ICalculatorFactory<TInput, TResult> calculatorFactory, INavigationService navigationService) : base(navigationService)
    {
        this.Calculator = calculatorFactory?.CreateCalculator() ?? throw new ArgumentNullException(nameof(calculatorFactory));
        this.Calculator.StateChanged += OnCalculatorStateChanged;
        this.CurrentState = ECalculatorState.NotStarted;
    }

    private async void OnCalculatorStateChanged(object? sender, CalculatorStateEventArgs e)
    {
        if (this.CurrentState == ECalculatorState.InProgress && e.State == ECalculatorState.Computed)
        {
            await Task.Delay(PROGRESS_INDICATOR_DELAY).ConfigureAwait(false);
        }
        this.CurrentState = e.State;
    }

    /// <summary>
    /// Handles input changes and triggers calculation if inputs are valid.
    /// </summary>
    protected void RefreshCalculation()
    {
        TInput input = this.GetInput();
        if (this.CanCalculate(input))
        {
            this.Calculate(input);
        }
    }

    /// <summary>
    /// Determines whether a calculation can be started based on current conditions.
    /// </summary>
    protected virtual bool CanCalculate(TInput input)
    {
        return !base.HasErrors
            && this.InpuValidation(input)
            && this.Calculator.State != ECalculatorState.InProgress
            && (_lastResult == null || !_lastResult.Content.UsedInputs.Equals(input));
    }

    private void Calculate(TInput input)
    {
        _lastResult = this.Calculator.Start(input);

        if (_lastResult.IsSuccess && _lastResult.HasContent)
        {
            this.OnCalculationSuccessfull(_lastResult);
        }
    }

    /// <summary>
    /// Creates and returns the calculation input based on current view model state.
    /// </summary>
    protected abstract TInput GetInput();

    /// <summary>
    /// Processes successful calculation results and updates the view model state.
    /// </summary>
    protected abstract void OnCalculationSuccessfull(OperationResult<TResult> result);

    /// <summary>
    /// Validates that all required inputs have valid values.
    /// </summary>
    private bool InpuValidation(TInput input)
    {
        ICalculatorInputValidation<TInput> validation = this.Calculator.GetValidation();
        if (!validation.Validate(input))
        {
            this.CalculatorInputErrors = string.Join(Environment.NewLine, validation.ErrorMessages);
            this.CurrentState = ECalculatorState.NotStarted;
            return false;
        }
        return true;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (this.Calculator != null)
            {
                this.Calculator.StateChanged -= OnCalculatorStateChanged;
            }
        }
        base.Dispose(disposing);
    }
}
