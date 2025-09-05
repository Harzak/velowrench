using velowrench.Core.Validation.Pipeline;

namespace velowrench.Calculations.Interfaces;

/// <summary>
/// Defines a builder for creating a validation pipeline for a specific input type.
/// </summary>
internal interface ICalculatorValidationBuilder<TInput> where TInput : class
{
    /// <summary>
    /// Constructs and returns a new instance of the <see cref="ValidationPipeline{TInput}"/> class.
    /// </summary>
    ValidationPipeline<TInput> Build();
}