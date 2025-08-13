using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Calculations.Exceptions;
using velowrench.Calculations.Interfaces;
using velowrench.Calculations.LogMessages;
using velowrench.Utils.Enums;
using velowrench.Utils.EventArg;
using velowrench.Utils.Results;

namespace velowrench.Calculations.Calculs;

/// <summary>
/// Abstract base class for all calculations, providing common functionality and state management.
/// </summary>
/// <typeparam name="TInput">The type of input data required for the calculation.</typeparam>
/// <typeparam name="TResult">The type of result returned by the calculation.</typeparam>
public abstract class BaseCalcul<TInput, TResult> : ICalcul<TInput, TResult> where TInput : class where TResult : class
{
    protected ILogger Logger { get; }
    protected abstract string CalculName { get; }
    
    /// <summary>
    /// Gets the current state of the calculation.
    /// </summary>
    public ECalculState State { get; private set; }
    
    /// <summary>
    /// Gets the last successful result produced by this calculation, or null if no successful calculation has been performed.
    /// </summary>
    public TResult? LastResult { get; protected set; }

    /// <summary>
    /// Event raised when the calculation state changes.
    /// </summary>
    public event EventHandler<CalculStateEventArgs>? StateChanged;

    public Func<ICalculInputValidation<TInput>> GetValidation { get; }

    protected BaseCalcul(Func<ICalculInputValidation<TInput>> validationProvider, ILogger logger)
    {
        this.GetValidation = validationProvider ?? throw new ArgumentNullException(nameof(validationProvider));
        this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.State = ECalculState.NotStarted;
    }

    /// <summary>
    /// Starts the calculation with the specified input data.
    /// </summary>
    /// <returns>An operation result containing the calculation result if successful, or error information if failed.</returns>
    /// <exception cref="InvalidCalculOperationException">Thrown when a calculation is already in progress.</exception>
    public OperationResult<TResult> Start(TInput input)
    {
        ArgumentNullException.ThrowIfNull(input, nameof(input));
        InvalidCalculOperationException.ThrowIfCalculInProgress(this);

        this.SetState(ECalculState.InProgress);

        OperationResult<TResult> result = Calculate(input);
        if (result.IsSuccess)
        {
            LastResult = result.Content;
        }
        else
        {
            string errorMessage = string.IsNullOrEmpty(result.ErrorMessage) ? "Unknown error" : result.ErrorMessage;    
            CalculLogs.CalculInError(this.Logger, errorMessage);
        }

        this.SetState(result.IsSuccess ? ECalculState.Computed : ECalculState.Failed);
        return result;
    }

    protected abstract OperationResult<TResult> Calculate(TInput input);

    protected void SetState(ECalculState state)
    {
        State = state;
        CalculLogs.CalculStateChangedAt(this.Logger, this.CalculName, state, DateTime.UtcNow);
        StateChanged?.Invoke(this, new CalculStateEventArgs(state));
    }
}