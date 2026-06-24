namespace velowrench.Utils.Results;

/// <summary>
/// Represents the result of an operation with success/failure status and error handling capabilities.
/// </summary>
[Serializable]
public class OperationResult : ResultBase<OperationResult>
{
    public OperationResult() : base()
    {

    }

    public OperationResult(bool success) : base(success)
    {

    }

    /// <summary>
    /// Creates a successful operation result.
    /// </summary>
    /// <returns>A new <see cref="OperationResult"/> instance representing a successful operation.</returns>
    public static OperationResult Success()
    {
        return new OperationResult(true);
    }

    /// <summary>
    /// Creates a failed operation result.
    /// </summary>
    /// <returns>A new <see cref="OperationResult"/> instance representing a failed operation.</returns>
    public static OperationResult Failure()
    {
        return new OperationResult(false);
    }

    /// <summary>
    /// Creates a failed operation result with an error message.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <returns>A new <see cref="OperationResult"/> instance representing a failed operation with the specified error message.</returns>
    public static OperationResult Failure(string message)
    {
        return new OperationResult(false).WithError(message);
    }

    /// <summary>
    /// Executes <paramref name="action"/> and returns a successful result if it completes without throwing,
    /// or a failed result with the caught exception if it throws.
    /// </summary>
    /// <returns>An <see cref="OperationResult"/> reflecting the outcome of <paramref name="action"/>.</returns>
    public static OperationResult Try(Action action)
    {
        ArgumentNullException.ThrowIfNull(action);

        try
        {
            action();
            return OperationResult.Success();
        }
        catch (Exception ex)
        {
            return OperationResult.Failure().WithException(ex);
        }
    }

    /// <summary>
    /// Asynchronously executes <paramref name="action"/> and returns a successful result if it completes without throwing,
    /// or a failed result with the caught exception if it throws.
    /// </summary>
    /// <returns>An <see cref="OperationResult"/> reflecting the outcome of <paramref name="action"/>.</returns>
    public static async Task<OperationResult> TryAsync(Func<Task> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        try
        {
            await action().ConfigureAwait(false);
            return OperationResult.Success();
        }
        catch (Exception ex)
        {
            return OperationResult.Failure().WithException(ex);
        }
    }

    /// <summary>
    /// Executes <paramref name="func"/> and returns a successful result carrying its return value if it completes without throwing,
    /// or a failed result with the caught exception if it throws.
    /// </summary>
    /// <returns>An <see cref="OperationResult{T}"/> reflecting the outcome of <paramref name="func"/>.</returns>
    public static OperationResult<T> Try<T>(Func<T> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        try
        {
            return new OperationResult<T>(func(), true);
        }
        catch (Exception ex)
        {
            return new OperationResult<T>().WithException(ex);
        }
    }

    /// <summary>
    /// Asynchronously executes <paramref name="func"/> and returns a successful result carrying its return value if it completes without throwing,
    /// or a failed result with the caught exception if it throws.
    /// </summary>
    /// <returns>An <see cref="OperationResult{T}"/> reflecting the outcome of <paramref name="func"/>.</returns>
    public static async Task<OperationResult<T>> TryAsync<T>(Func<Task<T>> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        try
        {
            return new OperationResult<T>(await func().ConfigureAwait(false), true);
        }
        catch (Exception ex)
        {
            return new OperationResult<T>().WithException(ex);
        }
    }
}
