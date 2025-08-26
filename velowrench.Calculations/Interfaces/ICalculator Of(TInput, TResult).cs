using velowrench.Utils.Results;

namespace velowrench.Calculations.Interfaces;


/// <summary>
/// Generic interface for calculations that process input data and return results.
/// </summary>
/// <typeparam name="TInput">The type of input data required for the calculation.</typeparam>
/// <typeparam name="TResult">The type of result returned by the calculation.</typeparam>
public interface ICalculator<TInput, TResult> : ICalculator where TInput : class
{
    /// <summary>
    /// Starts the calculation with the specified input data.
    /// </summary>
    /// <param name="input">The input data for the calculation.</param>
    /// <returns>An operation result containing the calculation result if successful, or error information if failed.</returns>
    OperationResult<TResult> Start(TInput input);

    public abstract Interfaces.ICalculatorInputValidator<TInput> InputValidator { get; }
}