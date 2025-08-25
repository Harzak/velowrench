using System.Runtime.CompilerServices;
using velowrench.Calculations.Interfaces;

[assembly: InternalsVisibleTo("velowrench.Calculations.Tests")]

namespace velowrench.Calculations.Calculators.Transmission.Chain;

/// <summary>
/// Provides validation logic for <see cref="ChainLengthCalculatorInput"/> instances, ensuring that input parameters
/// conform to defined business rules for chain length calculation.
/// </summary>
internal sealed class ChainLengthCalculatorInputValidator : ICalculatorInputValidation<ChainLengthCalculatorInput>
{
    private readonly Dictionary<string, string> _errorMessage;

    /// <summary>
    /// The minimum allowable chainstay length in inches.
    /// </summary>
    internal const double MIN_CHAINSTAY_LENGTH_INCH = 1.0;

    /// <summary>
    /// The minimum allowable number of teeth on a chainring.
    /// </summary>
    internal const int MIN_CHAINRING_TEETH_COUNT = 10;

    /// <summary>
    /// The maximum allowable number of teeth on a chainring.
    /// </summary>
    internal const int MAX_CHAINRING_TEETH_COUNT = 120;

    /// <summary>
    /// The minimum allowable number of teeth on a sprocket.
    /// </summary>
    internal const int MIN_SPROCKET_TEETH_COUNT = 9;

    /// <summary>
    /// The maximum allowable number of teeth on a sprocket.
    /// </summary>
    internal const int MAX_SPROCKET_TEETH_COUNT = 54;

    /// <summary>
    /// Gets a collection of validation error messages from the most recent validation attempt.
    /// </summary>
    public Dictionary<string, string> ErrorMessages => _errorMessage;

    public ChainLengthCalculatorInputValidator()
    {
        _errorMessage = [];
    }

    /// <summary>
    /// Validates chain length calculation input parameters against business rules.
    /// </summary>
    public bool Validate(ChainLengthCalculatorInput input)
    {
        if (input == null)
        {
            _errorMessage.Add(
                nameof(input),
                "Input cannot be null.");
            return false;
        }

        if (!ChainStayLengthIsValid(input.ChainStayLengthIn))
        {
            _errorMessage.Add(
                nameof(input.ChainStayLengthIn),
                $"Chainstay length must be greater than {MIN_CHAINSTAY_LENGTH_INCH} inch.");
        }

        if (!ChainringTeethCountIsValid(input.TeethLargestChainring))
        {
            _errorMessage.Add(
                nameof(input.TeethLargestChainring),
                $"Chainring teeth count must be between {MIN_CHAINRING_TEETH_COUNT} and {MAX_CHAINRING_TEETH_COUNT}.");
        }

        if (!SprocketTeethCountIsValid(input.TeethLargestSprocket))
        {
            _errorMessage.Add(
                nameof(input.TeethLargestSprocket),
                $"Sprocket teeth count must be between {MIN_SPROCKET_TEETH_COUNT} and {MAX_SPROCKET_TEETH_COUNT}.");
        }

        return _errorMessage.Count == 0;
    }

    private bool ChainStayLengthIsValid(double chainStayLength)
    {
        return chainStayLength >= MIN_CHAINSTAY_LENGTH_INCH;
    }

    private bool ChainringTeethCountIsValid(int teethCount)
    {
        return teethCount >= MIN_CHAINRING_TEETH_COUNT && teethCount <= MAX_CHAINRING_TEETH_COUNT;
    }

    private bool SprocketTeethCountIsValid(int teethCount)
    {
        return teethCount >= MIN_SPROCKET_TEETH_COUNT && teethCount <= MAX_SPROCKET_TEETH_COUNT;
    }
}
