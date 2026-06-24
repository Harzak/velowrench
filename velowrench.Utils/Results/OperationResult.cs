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
}
