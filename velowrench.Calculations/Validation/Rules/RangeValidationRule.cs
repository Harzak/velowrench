using velowrench.Calculations.Validation.Results;
using velowrench.Core.Validation;

namespace velowrench.Calculations.Validation.Rules;

/// <summary>
/// A validation rule that checks if a numeric value falls within a specified range.
/// </summary>
internal sealed class RangeValidationRule<TInput> : BaseValidationRule<TInput>
{
    private readonly double _minValue;
    private readonly double _maxValue;
    private readonly string _errorMessageTemplate;
    private readonly string? _unitName;

    public RangeValidationRule(
        string propertyName,
        double minValue,
        double maxValue,
        string errorMessageTemplate,
        string? unitName = null)
        : base(propertyName, $"Range_{propertyName}")
    {
        _minValue = minValue;
        _maxValue = maxValue;
        _errorMessageTemplate = errorMessageTemplate;
        _unitName = unitName;
    }


    /// <inheritdoc/>
    public override ValidationResult Validate(TInput input, object? value, ValidationContext context)
    {
        if (value is null)
        {
            return base.WithSuccess();
        }

        double numericValue = value switch
        {
            double d => d,
            float f => f,
            int i => i,
            long l => l,
            decimal dec => (double)dec,
            _ => throw new ArgumentException($"Value of type {value.GetType()} cannot be validated by RangeValidationRule")
        };

        if (numericValue < _minValue || numericValue > _maxValue)
        {
            string message = _unitName != null
                            ? string.Format(context.Culture, _errorMessageTemplate, _minValue, _maxValue, _unitName)
                            : string.Format(context.Culture, _errorMessageTemplate, _minValue, _maxValue);

            return base.WithError(message, new { MinValue = _minValue, MaxValue = _maxValue, ActualValue = numericValue });
        }

        return base.WithSuccess();
    }
}
