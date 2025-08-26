namespace velowrench.Calculations.Validation.Results;

/// <summary>
/// Represents a validation error with contextual information.
/// </summary>
public sealed record ValidationError
{
    public required string PropertyName { get; init; }
    public required string Message { get; init; }
}

public static class ValidationErrorExtensions
{
    public static bool TryGetMessage(this IEnumerable<ValidationError> errors, string propertyName, out string? message)
    {
        ValidationError? error = errors.FirstOrDefault(e => e.PropertyName == propertyName);
        if (error != null)
        {
            message = error.Message;
            return true;
        }
        message = null;
        return false;
    }
}
