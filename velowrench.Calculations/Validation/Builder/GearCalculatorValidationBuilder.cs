using velowrench.Calculations.Calculators.Transmission.Gear;
using velowrench.Calculations.Enums;
using velowrench.Calculations.Validation.Results;

namespace velowrench.Calculations.Validation.Builder;

/// <summary>
/// Fluent builder for gear calculator validation rules.
/// </summary>
internal sealed class GearCalculatorValidationBuilder : BaseCalculatorValidationBuilder<GearCalculatorInput>
{
    /// <summary>
    /// The minimum allowable crank length in millimeters.
    /// </summary>
    public const int MIN_CRANK_LENGTH_MM = 100;

    /// <summary>
    /// The maximum allowable crank length in millimeters.
    /// </summary>
    public const int MAX_CRANK_LENGTH_MM = 190;

    /// <summary>
    /// The minimum allowable wheel diameter in inches.
    /// </summary>
    public const int MIN_TYRE_DIAMETER_INCH = 7;

    /// <summary>
    /// The maximum allowable wheel diameter in inches.
    /// </summary>
    public const int MAX_TYRE_DIAMETER_INCH = 38;

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
    public const int MAX_SPROCKET_TEETH_COUNT = 52;

    /// <summary>
    /// The maximum number of sprockets allowed in a single calculation.
    /// </summary>
    public const int MAX_SPROCKETS_COUNT = 15;

    /// <summary>
    /// The minimum allowable revolutions per minute for speed calculations.
    /// </summary>
    public const int MIN_RPM = 20;

    /// <summary>
    /// The maximum allowable revolutions per minute for speed calculations.
    /// </summary>
    public const int MAX_RPM = 150;

    public GearCalculatorValidationBuilder(ValidationContext? validationContext = null) : base(validationContext)
    {

    }

    /// <inheritdoc/>
    protected override void ConfigureValidationRules()
    {
        HasRange(
            nameof(GearCalculatorInput.TyreOuterDiameterIn),
            MIN_TYRE_DIAMETER_INCH,
            MAX_TYRE_DIAMETER_INCH,
            "Tyre outer diameter must be between {0} and {1} {2}.",
            "inches");

        HasRange(
            nameof(GearCalculatorInput.TeethNumberLargeOrUniqueChainring),
            MIN_CHAINRING_TEETH_COUNT,
            MAX_CHAINRING_TEETH_COUNT,
            "Chainring teeth count must be between {0} and {1}.");

        HasCustomRule(
            nameof(GearCalculatorInput.TeethNumberMediumChainring),
            "MediumChainringRange",
            (input, value, context) =>
            {
                if (value is int mediumChainring && (mediumChainring < MIN_CHAINRING_TEETH_COUNT || mediumChainring > MAX_CHAINRING_TEETH_COUNT))
                {
                    return ValidationResult.WithError(
                        nameof(GearCalculatorInput.TeethNumberMediumChainring),
                        $"Medium chainring teeth count must be between {MIN_CHAINRING_TEETH_COUNT} and {MAX_CHAINRING_TEETH_COUNT}.");
                }
                return ValidationResult.WithSuccess();
            });

        HasCustomRule(
            nameof(GearCalculatorInput.TeethNumberSmallChainring),
            "SmallChainringRange",
            (input, value, context) =>
            {
                if (value is int smallChainring && (smallChainring < MIN_CHAINRING_TEETH_COUNT || smallChainring > MAX_CHAINRING_TEETH_COUNT))
                {
                    return ValidationResult.WithError(
                        nameof(GearCalculatorInput.TeethNumberSmallChainring),
                        $"Small chainring teeth count must be between {MIN_CHAINRING_TEETH_COUNT} and {MAX_CHAINRING_TEETH_COUNT}.");

                }
                return ValidationResult.WithSuccess();
            });

        HasCustomRule(
            nameof(GearCalculatorInput.CrankLengthMm),
            "CrankLengthForGainRatio",
            (input, value, context) =>
            {
                if (input.CalculatorType == EGearCalculatorType.GainRatio)
                {
                    if (value is double crankLength)
                    {
                        if (crankLength < MIN_CRANK_LENGTH_MM || crankLength > MAX_CRANK_LENGTH_MM)
                        {
                            return ValidationResult.WithError(
                                nameof(GearCalculatorInput.CrankLengthMm),
                                $"Crank length must be between {MIN_CRANK_LENGTH_MM} and {MAX_CRANK_LENGTH_MM} mm for gain ratio calculations.");
                        }
                    }
                }
                return ValidationResult.WithSuccess();
            });

        HasCustomRule(
            nameof(GearCalculatorInput.RevolutionPerMinute),
            "RPMForSpeed",
            (input, value, context) =>
            {
                if (input.CalculatorType == EGearCalculatorType.Speed)
                {
                    if (!input.RevolutionPerMinute.HasValue)
                    {
                        return ValidationResult.WithError(
                            nameof(GearCalculatorInput.RevolutionPerMinute),
                            "Revolution per minute is required for speed calculations.");
                    }
                    else if (input.RevolutionPerMinute.Value < MIN_RPM || input.RevolutionPerMinute.Value > MAX_RPM)
                    {
                        return ValidationResult.WithError(
                            nameof(GearCalculatorInput.RevolutionPerMinute),
                            $"Revolution per minute must be between {MIN_RPM} and {MAX_RPM} for speed calculations.");
                    }
                }
                return ValidationResult.WithSuccess();
            });

        HasCustomRule(
            nameof(GearCalculatorInput.NumberOfTeethBySprocket),
            "SprocketsValidation",
            (input, value, context) =>
            {
                if (input.NumberOfTeethBySprocket.Count < 1)
                {
                    return ValidationResult.WithError(
                        nameof(GearCalculatorInput.NumberOfTeethBySprocket),
                        "At least one sprocket is required.");
                }

                if (input.NumberOfTeethBySprocket.Count > MAX_SPROCKETS_COUNT)
                {
                    return ValidationResult.WithError(
                        nameof(GearCalculatorInput.NumberOfTeethBySprocket),
                        $"Maximum {MAX_SPROCKETS_COUNT} sprockets allowed ({input.NumberOfTeethBySprocket.Count} entered).");
                }

                var invalidSprockets = input.NumberOfTeethBySprocket
                    .Where(teeth => teeth < MIN_SPROCKET_TEETH_COUNT || teeth > MAX_SPROCKET_TEETH_COUNT)
                    .ToList();

                if (invalidSprockets.Count > 0)
                {
                    return ValidationResult.WithError(
                        nameof(GearCalculatorInput.NumberOfTeethBySprocket),
                        $"Sprocket teeth counts must be between {MIN_SPROCKET_TEETH_COUNT} and {MAX_SPROCKET_TEETH_COUNT}.");
                }

                return ValidationResult.WithSuccess();
            });
    }
}