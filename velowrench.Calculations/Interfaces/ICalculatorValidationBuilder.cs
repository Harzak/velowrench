using velowrench.Core.Validation.Pipeline;

namespace velowrench.Calculations.Interfaces;

internal interface ICalculatorValidationBuilder<TInput> where TInput : class
{
    ValidationPipeline<TInput> Build();
}