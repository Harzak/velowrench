using velowrench.Calculations.Interfaces;
using velowrench.Calculations.Validation.Results;
using velowrench.Calculations.Validation.Rules;
using velowrench.Core.Validation;
using velowrench.Core.Validation.Pipeline;

namespace velowrench.Calculations.Validation.Builder;


/// <summary>
/// Base class for fluent validation pipeline builders.
/// </summary>
internal abstract class BaseCalculatorValidationBuilder<TInput> : ICalculatorValidationBuilder<TInput> where TInput : class
{
    protected ValidationPipeline<TInput> Pipeline { get; }

    protected BaseCalculatorValidationBuilder(ValidationContext? validationContext = null)
    {
        this.Pipeline = new ValidationPipeline<TInput>(validationContext);
    }

    /// <inheritdoc/>
    public ValidationPipeline<TInput> Build()
    {
        ConfigureValidationRules();
        return Pipeline;
    }

    /// <summary>
    /// Configures the validation rules.
    /// </summary>
    protected abstract void ConfigureValidationRules();

    /// <summary>
    /// Adds a range validation rule for a numeric property.
    /// </summary>
    protected BaseCalculatorValidationBuilder<TInput> HasRange(string propertyName, double minValue, double maxValue, string errorMessageTemplate, string? unitName = null)
    {
        IValidationRule<TInput> rule = new RangeValidationRule<TInput>(propertyName, minValue, maxValue, errorMessageTemplate, unitName);
        return AddRule(rule);
    }

    /// <summary>
    /// Adds a required validation rule for a property.
    /// </summary>
    protected BaseCalculatorValidationBuilder<TInput> IsRequired(string propertyName, string errorMessage)
    {
        IValidationRule<TInput> rule = new RequiredValidationRule<TInput>(propertyName, errorMessage);
        return AddRule(rule);
    }

    /// <summary>
    /// Adds a custom validation rule for a property.
    /// </summary>
    protected BaseCalculatorValidationBuilder<TInput> HasCustomRule(string propertyName, string ruleName, Func<TInput, object?, ValidationContext, ValidationResult> validationFunc)
    {
        IValidationRule<TInput> rule = new CustomValidationRule<TInput>(propertyName, ruleName, validationFunc);
        return AddRule(rule);
    }

    private BaseCalculatorValidationBuilder<TInput> AddRule(IValidationRule<TInput> rule)
    {
        this.Pipeline.AddRule(rule);
        return this;
    }
}