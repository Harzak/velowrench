namespace velowrench.Calculations.Interfaces;

/// <summary>
/// Factory interface for creating calculation instances.
/// </summary>
/// <typeparam name="TInput">The type of input data required for the calculation.</typeparam>
/// <typeparam name="TResult">The type of result returned by the calculation.</typeparam>
public interface ICalculatorFactory<TInput, TResult> where TInput : class where TResult : class
{
    /// <summary>
    /// Creates a new instance of a calculation that processes the specified input and returns the specified result type.
    /// </summary>
    /// <returns>A new calculation instance ready to process input data.</returns>
    ICalculator<TInput, TResult> CreateCalculator();
}