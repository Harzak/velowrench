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
    private readonly List<string> _errors = [];

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
    /// Gets the list of functional/user error messages concatenated.
    /// </summary>
    public IReadOnlyList<string> Errors => _errors.AsReadOnly();

    /// <summary>
    /// Gets the concatenated functional/user error message (joined by <see cref="Environment.NewLine"/>).
    /// </summary>
    public string ErrorMessage => string.Join(Environment.NewLine, _errors);

    /// <summary>
    /// Gets a value indicating whether this result has any functional error messages.
    /// </summary>
    public bool HasErrors => _errors.Count > 0;

    /// <summary>
    /// Gets the technical exception associated with this result.
    /// </summary>
    public Exception? Exception { get; private set; }

    /// <summary>
    /// Gets a value indicating whether a technical exception is associated with this result.
    /// </summary>
    public bool HasException => Exception is not null;

    /// <summary>
    /// Gets or sets the error code when the operation fails.
    /// </summary>
    public string ErrorCode { get; set; }

    protected ResultBase(bool success)
    {
        IsSuccess = success;
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
    /// Adds a functional error message and sets the operation result to failed status.
    /// </summary>
    /// <returns>The current <see cref="TResult"/> instance with the failure status and error message added.</returns>
    public TResult WithError(string message)
    {
        if (!string.IsNullOrEmpty(message))
        {
            _errors.Add(message);
        }
        return this.WithFailure();
    }

    /// <summary>
    /// Adds multiple functional error messages and sets the operation result to failed status.
    /// </summary>
    /// <returns>The current <see cref="TResult"/> instance with the failure status and error messages added.</returns>
    public TResult WithErrors(IEnumerable<string> messages)
    {
        if (messages != null)
        {
            _errors.AddRange(messages.Where(m => !string.IsNullOrEmpty(m)));
        }
        return this.WithFailure();
    }

    /// <summary>
    /// Stores a technical exception and sets the operation result to failed status.
    /// </summary>
    /// <returns>The current <see cref="TResult"/> instance with the failure status and exception set.</returns>
    public TResult WithException(Exception exception)
    {
        ArgumentNullException.ThrowIfNull(exception);
        this.Exception = exception;
        return this.WithFailure();
    }

    /// <summary>
    /// Sets the error code and sets the operation result to failed status.
    /// </summary>
    /// <returns>The current <see cref="TResult"/> instance with the failure status and error code set.</returns>
    public TResult WithErrorCode(string code)
    {
        this.ErrorCode = code;
        return this.WithFailure();
    }

    /// <summary>
    /// Copies all accumulated error messages and the technical exception from <paramref name="source"/> into this instance.
    /// </summary>
    internal void CopyErrorStateFrom<TSource>(ResultBase<TSource> source) where TSource : ResultBase<TSource>
    {
        if (source != null)
        {
            _errors.AddRange(source.Errors);
            if (source.Exception != null)
            {
                Exception = source.Exception;
            }
        }
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
    /// Affects this result with the full status and error state of another <see cref="ResultBase{TSource}"/> result.
    /// </summary>
    public TResult Affect<TSource>(ResultBase<TSource> result) where TSource : ResultBase<TSource>
    {
        if (result != null && IsSuccess)
        {
            IsSuccess = result.IsSuccess;
            this.CopyErrorStateFrom(result);
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
    /// Affects this result with the full status and error state of a <see cref="ResultBase{TSource}"/> produced by a function.
    /// </summary>
    public TResult Affect<TSource>(Func<ResultBase<TSource>> result) where TSource : ResultBase<TSource>
    {
        if (result != null)
        {
            return Affect(result());
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

    /// <summary>
    /// Asynchronously affects this result with the full status and error state of a <see cref="ResultBase{TSource}"/> produced by an async function.
    /// </summary>
    public async Task<TResult> AffectAsync<TSource>(Func<Task<ResultBase<TSource>>> result) where TSource : ResultBase<TSource>
    {
        if (result != null)
        {
            return this.Affect(await result().ConfigureAwait(false));
        }
        return (TResult)this;
    }

    /// <summary>
    /// Returns <see langword="true"/> when the operation completed successfully; otherwise <see langword="false"/>.
    /// </summary>
    public bool ToBoolean()
    {
        return this.IsSuccess;
    }

    /// <summary>
    /// Returns <see langword="true"/> when the operation completed successfully; otherwise <see langword="false"/>.
    /// </summary>
    public static implicit operator bool(ResultBase<TResult> result) => result?.IsSuccess ?? false;
}
