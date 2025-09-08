using velowrench.Calculations.Interfaces;
using velowrench.Calculations.Validation.Results;
using velowrench.Calculations.Validation;
using velowrench.Calculations.Validation.Pipeline;

namespace velowrench.Calculations.Validation;

/// <summary>
/// Provides validation for calculator input using a configurable validation pipeline.
/// </summary>
internal sealed class CalculatorInputValidator<TInput> : ICalculatorInputValidator<TInput> where TInput : class
{
    private readonly ValidationPipeline<TInput> _validationPipeline;

    public CalculatorInputValidator(ICalculatorValidationBuilder<TInput> validationBuilder)
    {
        _validationPipeline = validationBuilder.Build();
    }

    /// <inheritdoc />
    public bool Validate(TInput input)
    {
        ValidationResult result = ValidateWithResults(input);
        return result.IsValid;
    }

    /// <inheritdoc />
    public ValidationResult ValidateWithResults(TInput input, ValidationContext? context = null)
    {
        return _validationPipeline.ValidateRules(input, context);
    }

    /// <inheritdoc />
    public ValidationResult ValidateProperty(TInput input, string propertyName, ValidationContext? context = null)
    {
        return _validationPipeline.ValidateRulesForProperty(input, propertyName, context);
    }
}
