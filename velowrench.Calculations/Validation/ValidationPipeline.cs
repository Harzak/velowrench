using System.Reflection;
using velowrench.Calculations.Interfaces;
using velowrench.Calculations.Validation.Results;
using velowrench.Utils.Enums;

namespace velowrench.Calculations.Validation.Pipeline;

/// <summary>
/// A composable validation pipeline that applies multiple validation rules to an input object.
/// </summary>
internal sealed class ValidationPipeline<TInput>
{
    private readonly List<IValidationRule<TInput>> _rules;
    private readonly ValidationContext _defaultContext;

    public ValidationPipeline(ValidationContext? defaultContext = null)
    {
        _rules = [];
        _defaultContext = defaultContext ?? new ValidationContext(EValidationMode.Immediate);
    }

    internal ValidationPipeline<TInput> AddRule(IValidationRule<TInput> rule)
    {
        _rules.Add(rule);
        return this;
    }

    internal ValidationResult ValidateRules(TInput input, ValidationContext? context = null)
    {
        ValidationContext validationContext = context ?? _defaultContext;
        List<ValidationError> errors = [];

        foreach (IValidationRule<TInput> rule in _rules)
        {
            object? propertyValue = GetPropertyValue(input, rule.PropertyName);
            ValidationResult result = rule.Validate(input, propertyValue, validationContext);
            errors.AddRange(result.Errors);
        }

        return ValidationResult.WithErrors(errors);
    }

    internal ValidationResult ValidateRulesForProperty(TInput input, string propertyName, ValidationContext? context = null)
    {
        ValidationContext validationContext = context ?? _defaultContext;
        List<ValidationError> errors = [];

        IEnumerable<IValidationRule<TInput>> propertyRules = _rules.Where(r => r.PropertyName == propertyName);
        object? propertyValue = GetPropertyValue(input, propertyName);

        foreach (IValidationRule<TInput> rule in propertyRules)
        {
            ValidationResult result = rule.Validate(input, propertyValue, validationContext);
            errors.AddRange(result.Errors);
        }

        return ValidationResult.WithErrors(errors);
    }


    private object? GetPropertyValue(TInput input, string propertyName)
    {
        PropertyInfo? property = typeof(TInput).GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
        return property?.GetValue(input);
    }
}
