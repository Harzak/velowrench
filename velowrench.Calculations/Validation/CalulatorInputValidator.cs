using velowrench.Calculations.Interfaces;
using velowrench.Calculations.Validation.Results;
using velowrench.Core.Validation;
using velowrench.Core.Validation.Pipeline;

namespace velowrench.Calculations.Validation;

internal sealed class CalulatorInputValidator<TInput> : ICalculatorInputValidator<TInput> where TInput : class
{
    private readonly ValidationPipeline<TInput> _validationPipeline;

    public CalulatorInputValidator(ICalculatorValidationBuilder<TInput> validationBuilder)
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
