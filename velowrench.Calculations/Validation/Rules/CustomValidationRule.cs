using velowrench.Calculations.Validation.Results;
using velowrench.Core.Validation;

namespace velowrench.Calculations.Validation.Rules;

/// <summary>
/// A validation rule that applies custom validation logic.
/// </summary>
internal sealed class CustomValidationRule<TInput> : BaseValidationRule<TInput>
{
    private readonly Func<TInput, object?, ValidationContext, ValidationResult> _validationFunc;

    public CustomValidationRule(
        string propertyName,
        string ruleName,
        Func<TInput, object?, ValidationContext, ValidationResult> validationFunc)
        : base(propertyName, ruleName)
    {
        _validationFunc = validationFunc ?? throw new ArgumentNullException(nameof(validationFunc));
    }


    /// <inheritdoc/>
    public override ValidationResult Validate(TInput input, object? value, ValidationContext context)
    {
        return _validationFunc(input, value, context);
    }
}
