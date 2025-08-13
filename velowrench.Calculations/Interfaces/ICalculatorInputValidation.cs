namespace velowrench.Calculations.Interfaces;

/// <summary>
/// Defines a contract for validating calculation input parameters of type <typeparamref name="TInput"/>.
/// Provides validation logic and error reporting capabilities for ensuring input data meets calculation requirements.
/// </summary>
/// <typeparam name="TInput">The type of input data to be validated</typeparam>
public interface ICalculatorInputValidation<TInput>
{
    /// <summary>
    /// Gets a collection of error messages generated during the last validation operation.
    /// This collection is populated when <see cref="Validate"/> returns false.
    /// </summary>
    IEnumerable<string> ErrorMessages { get; }

    /// <summary>
    /// Validates the specified input data against defined business rules and constraints.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the input passes all validation rules; otherwise, <c>false</c>.
    /// When validation fails, error messages are available through <see cref="ErrorMessages"/>.
    /// </returns>
    bool Validate(TInput input);
}