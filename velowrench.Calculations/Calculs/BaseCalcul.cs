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

public abstract class BaseCalcul<TInput, TResult> : ICalcul<TInput, TResult> where TInput : class where TResult : class
{
    protected ILogger Logger { get; }
    protected abstract string CalculName { get; }
    public ECalculState State { get; private set; }
    public TResult? LastResult { get; protected set; }

    public event EventHandler<CalculStateEventArgs>? StateChanged;

    protected BaseCalcul(ILogger logger)
    {
        this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.State = ECalculState.NotStarted;
    }

    public OperationResult<TResult> Start(TInput input)
    {
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