using velowrench.Calculations.Validation;
using velowrench.Calculations.Validation.Results;

namespace velowrench.Calculations.Interfaces;

/// <summary>
/// Defines the contract for a validation rule that can be applied to an input property.
/// </summary>
internal interface IValidationRule<TInput>
{
    string PropertyName { get; }
    string RuleName { get; }

    /// <summary>
    /// Validates the specified input value against the provided validation context.
    /// </summary>
    ValidationResult Validate(TInput input, object? value, ValidationContext context);
}