using System.Runtime.CompilerServices;
using velowrench.Calculations.Validation.Results;
using velowrench.Core.Validation;

[assembly: InternalsVisibleTo("velowrench.Calculations.Tests")]

namespace velowrench.Calculations.Interfaces;

/// <summary>
/// Enhanced interface for calculator input validation with rich validation results.
/// </summary>
/// <typeparam name="TInput">The type of input to validate.</typeparam>
public interface ICalculatorInputValidator<TInput>
{
    /// <summary>
    /// Validates the specified input based on predefined criteria.
    /// </summary>
    bool Validate(TInput input);
    /// <summary>
    /// Validates the specified input data and returns detailed validation results.
    /// </summary>
    ValidationResult ValidateWithResults(TInput input, ValidationContext? context = null);

    /// <summary>
    /// Validates a specific property of the input data.
    /// </summary>
    ValidationResult ValidateProperty(TInput input, string propertyName, ValidationContext? context = null);
}
