using Microsoft.Extensions.Logging;
using velowrench.Calculations.Exceptions;
using velowrench.Calculations.Interfaces;
using velowrench.Calculations.LogMessages;
using velowrench.Calculations.Validation.Results;
using velowrench.Core.Validation.Pipeline;
using velowrench.Utils.Enums;
using velowrench.Utils.EventArg;
using velowrench.Utils.Results;

namespace velowrench.Calculations.Calculators;

/// <summary>
/// Abstract base class for all calculations, providing common functionality and state management.
/// </summary>
/// <typeparam name="TInput">The type of input data required for the calculation.</typeparam>
/// <typeparam name="TResult">The type of result returned by the calculation.</typeparam>
public abstract class BaseCalculator<TInput, TResult> : ICalculator<TInput, TResult> where TInput : class where TResult : class
{
    protected ILogger Logger { get; }
    protected abstract string CalculatorName { get; }

    /// <summary>
    /// Gets the input validator used to validate the input used for the calculation.
    /// </summary>
    public abstract ICalculatorInputValidator<TInput> InputValidator { get; }

    /// <summary>
    /// Gets the current state of the calculation.
    /// </summary>
    public ECalculatorState State { get; private set; }

    /// <summary>
    /// Gets the last successful result produced by this calculation, or null if no successful calculation has been performed.
    /// </summary>
    public TResult? LastResult { get; protected set; }

    /// <summary>
    /// Event raised when the calculation state changes.
    /// </summary>
    public event EventHandler<CalculatorStateEventArgs>? StateChanged;

    protected BaseCalculator(ILogger logger)
    {
        this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.State = ECalculatorState.NotStarted;
    }

    /// <summary>
    /// Starts the calculation with the specified input data.
    /// </summary>
    /// <returns>An operation result containing the calculation result if successful, or error information if failed.</returns>
    /// <exception cref="InvalidCalculatorOperationException">Thrown when a calculation is already in progress.</exception>
    public OperationResult<TResult> Start(TInput input)
    {
        ArgumentNullException.ThrowIfNull(input, nameof(input));
        InvalidCalculatorOperationException.ThrowIfCalculInProgress(this);

        ValidationResult validationResult = this.InputValidator.ValidateWithResults(input);
        if (!validationResult.IsValid)
        {
            throw new CalculatorInputException(validationResult.ErrorMessages);
        }

        this.SetState(ECalculatorState.InProgress);

        OperationResult<TResult> result = this.Calculate(input);
        if (result.IsSuccess)
        {
            LastResult = result.Content;
        }
        else
        {
            string errorMessage = string.IsNullOrEmpty(result.ErrorMessage) ? "Unknown error" : result.ErrorMessage;
            CalculatorLogs.CalculationInError(this.Logger, errorMessage);
        }

        this.SetState(result.IsSuccess ? ECalculatorState.Computed : ECalculatorState.Failed);
        return result;
    }

    protected abstract OperationResult<TResult> Calculate(TInput input);

    protected void SetState(ECalculatorState state)
    {
        State = state;
        CalculatorLogs.CalculationStateChangedAt(this.Logger, this.CalculatorName, state, DateTime.UtcNow);
        StateChanged?.Invoke(this, new CalculatorStateEventArgs(state));
    }
}