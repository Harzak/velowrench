using velowrench.Calculations.Interfaces;
using velowrench.Calculations.Validation.Results;
using velowrench.Calculations.Validation;

namespace velowrench.Calculations.Validation.Rules;

/// <summary>
/// Base class for validation rules providing common functionality.
/// </summary>
internal abstract class BaseValidationRule<TInput> : IValidationRule<TInput>
{
    public string PropertyName { get; }
    public string RuleName { get; }

    protected BaseValidationRule(string propertyName, string ruleName)
    {
        this.PropertyName = propertyName;
        this.RuleName = ruleName;
    }

    /// <inheritdoc/>
    public abstract ValidationResult Validate(TInput input, object? value, ValidationContext context);

    protected ValidationResult WithError(string message, object? context = null)
    {
        ValidationError error = new()
        {
            PropertyName = this.PropertyName,
            Message = message
        };
        return ValidationResult.WithErrors([error]);
    }

    protected ValidationResult WithSuccess()
    {
        return ValidationResult.WithSuccess();
    }
}
