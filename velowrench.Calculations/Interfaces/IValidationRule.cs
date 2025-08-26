using velowrench.Calculations.Validation.Results;
using velowrench.Core.Validation;

namespace velowrench.Calculations.Interfaces;

/// <summary>
/// Defines the contract for a validation rule that can be applied to an input property.
/// </summary>
internal interface IValidationRule<TInput>
{
    string PropertyName { get; }
    string RuleName { get; }
    ValidationResult Validate(TInput input, object? value, ValidationContext context);
}