using velowrench.Calculations.Validation.Results;
using velowrench.Core.Validation;

namespace velowrench.Calculations.Validation.Rules;

/// <summary>
/// A validation rule that checks if a value is required (not null or empty).
/// </summary>
internal sealed class RequiredValidationRule<TInput> : BaseValidationRule<TInput>
{
    private readonly string _errorMessage;

    public RequiredValidationRule(
        string propertyName,
        string errorMessage)
        : base(propertyName, $"Required_{propertyName}")
    {
        _errorMessage = errorMessage;
    }


    /// <inheritdoc/>
    public override ValidationResult Validate(TInput input, object? value, ValidationContext context)
    {
        if (value is null)
        {
            return base.WithError(_errorMessage);
        }

        if (value is string str && string.IsNullOrWhiteSpace(str))
        {
            return base.WithError(_errorMessage);
        }

        if (value is System.Collections.ICollection collection && collection.Count == 0)
        {
            return base.WithError(_errorMessage);
        }

        return base.WithSuccess();
    }
}
