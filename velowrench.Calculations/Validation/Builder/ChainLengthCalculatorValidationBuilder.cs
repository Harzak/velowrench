using velowrench.Calculations.Calculators.Transmission.Chain;
using velowrench.Core.Validation;

namespace velowrench.Calculations.Validation.Builder;

/// <summary>
/// Fluent builder for chain length calculator validation rules.
/// </summary>
internal sealed class ChainLengthcalculatorValidationBuilder : BaseCalculatorValidationBuilder<ChainLengthCalculatorInput>
{
    /// <summary>
    /// The minimum allowable chainstay length in inches.
    /// </summary>
    public const double MIN_CHAINSTAY_LENGTH_INCH = 1.0;

    /// <summary>
    /// The minimum allowable number of teeth on a chainring.
    /// </summary>
    public const int MIN_CHAINRING_TEETH_COUNT = 10;

    /// <summary>
    /// The maximum allowable number of teeth on a chainring.
    /// </summary>
    public const int MAX_CHAINRING_TEETH_COUNT = 120;

    /// <summary>
    /// The minimum allowable number of teeth on a sprocket.
    /// </summary>
    public const int MIN_SPROCKET_TEETH_COUNT = 9;

    /// <summary>
    /// The maximum allowable number of teeth on a sprocket.
    /// </summary>
    public const int MAX_SPROCKET_TEETH_COUNT = 54;

    public ChainLengthcalculatorValidationBuilder(ValidationContext? validationContext = null) : base(validationContext)
    {

    }

    protected override void ConfigureValidationRules()
    {
        HasRange(
            nameof(ChainLengthCalculatorInput.ChainStayLengthIn),
            MIN_CHAINSTAY_LENGTH_INCH,
            double.MaxValue,
            "Chainstay length must be greater than {0} {2}.",
            "inch");

        HasRange(
            nameof(ChainLengthCalculatorInput.TeethLargestChainring),
            MIN_CHAINRING_TEETH_COUNT,
            MAX_CHAINRING_TEETH_COUNT,
            "Chainring teeth count must be between {0} and {1}.");

        HasRange(
            nameof(ChainLengthCalculatorInput.TeethLargestSprocket),
            MIN_SPROCKET_TEETH_COUNT,
            MAX_SPROCKET_TEETH_COUNT,
            "Sprocket teeth count must be between {0} and {1}.");
    }
}