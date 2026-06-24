using velowrench.Utils.Interfaces;

namespace velowrench.Utils.Results;

/// <summary>
/// Provides an abstract base class for operation results with success/failure status and error handling.
/// </summary>
public abstract class ResultBase<TResult> : IResult 
    where TResult : ResultBase<TResult>
{
    private bool _isSuccess;
    private bool _isFailed;

    /// <summary>
    /// Gets or sets a value indicating whether the operation completed successfully.
    /// </summary>
    public bool IsSuccess
    {
        get => _isSuccess;
        set
        {
            _isSuccess = value;
            _isFailed = !value;
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the operation failed.
    /// </summary>
    public bool IsFailed
    {
        get => _isFailed;
        set
        {
            _isFailed = value;
            _isSuccess = !value;
        }
    }

    /// <summary>
    /// Gets or sets the error message when the operation fails.
    /// </summary>
    public string ErrorMessage { get; set; }

    /// <summary>
    /// Gets or sets the error code when the operation fails.
    /// </summary>
    public string ErrorCode { get; set; }

    protected ResultBase(bool success)
    {
        IsSuccess = success;
        this.ErrorMessage = string.Empty;
        this.ErrorCode = string.Empty;
    }

    protected ResultBase() : this(false)
    {

    }

    /// <summary>
    /// Sets the operation result to successful status.
    /// </summary>
    /// <returns>The current <see cref="TResult"/> instance with the success status set.</returns>
    public TResult WithSuccess()
    {
        this.IsSuccess = true;
        return (TResult)this;
    }

    /// <summary>
    /// Sets the operation result to failed status.
    /// </summary>
    /// <returns>The current <see cref="TResult"/> instance with the failure status set.</returns>
    public TResult WithFailure()
    {
        this.IsSuccess = false;
        return (TResult)this;
    }

    /// <summary>
    /// Sets the operation result to failed status with an error message.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <returns>The current <see cref="TResult"/> instance with the failure status and error message set.</returns>
    public TResult WithError(string message)
    {
        this.ErrorMessage = message;
        return this.WithFailure();
    }

    /// <summary>
    /// Affects this result with the success status of another result.
    /// </summary>
    public TResult Affect(IResult result)
    {
        if (result != null && IsSuccess)
        {
            IsSuccess = result.IsSuccess;
        }
        return (TResult)this;
    }

    /// <summary>
    /// Affects this result with the success status of a result produced by a function.
    /// </summary>
    public TResult Affect(Func<IResult> result)
    {
        if (result != null)
        {
            return (TResult)Affect(result());
        }
        return (TResult)this;
    }

    /// <summary>
    /// Asynchronously affects this result with the success status of a result produced by an async function.
    /// </summary>
    public async Task<TResult> AffectAsync(Func<Task<IResult>> result)
    {
        if (result != null)
        {
            return this.Affect(await result().ConfigureAwait(false));
        }
        return (TResult)this;
    }
}
