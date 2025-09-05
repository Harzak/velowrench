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
    /// <returns>An operation result containing the calculation result if successful, or error information if failed.</returns>
    OperationResult<TResult> Start(TInput input);

    /// <summary>
    /// Gets the input validator used to validate the input used for the calculation.
    /// </summary>
    public abstract ICalculatorInputValidator<TInput> InputValidator { get; }
}