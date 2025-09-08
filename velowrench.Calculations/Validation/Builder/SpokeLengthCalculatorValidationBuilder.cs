using velowrench.Calculations.Calculators.Wheels.SpokeLength;
using velowrench.Calculations.Validation;

namespace velowrench.Calculations.Validation.Builder;

/// <summary>
/// Fluent builder for spoke length calculator validation rules.
/// </summary>
internal sealed class SpokeLengthCalculatorValidationBuilder : BaseCalculatorValidationBuilder<SpokeLengthCalculatorInput>
{
    /// <summary>
    /// The minimum allowable diameter, in millimeters, for a hub flange.
    /// </summary>
    public const int MIN_HUB_FLANGE_DIAMETER_MM = 20;

    /// <summary>
    /// The maximum allowable diameter, in millimeters, for a hub flange.
    /// </summary>
    public const int MAX_HUB_FLANGE_DIAMETER_MM = 80;

    /// <summary>
    /// The minimum allowable distance, in millimeters, between the hub center and the flange.
    /// </summary>
    public const int MIN_HUB_CENTER_TO_FLANGE_DISTANCE_MM = 10;

    /// <summary>
    /// The maximum allowable distance, in millimeters, between the hub center and the flange.
    /// </summary>
    public const int MAX_HUB_CENTER_TO_FLANGE_DISTANCE_MM = 60;

    /// <summary>
    /// The minimum Effective Rim Diameter (ERD) in millimeters.
    /// </summary>
    public const int MIN_ERD_MM = 200;

    /// <summary>
    /// The maximum allowable size, in millimeters, for the ERD (Effective Radiated Diameter).
    /// </summary>
    public const int MAX_ERD_MM = 800;

    /// <summary>
    /// The minimum number of spokes required for a valid configuration.
    /// </summary>
    public const int MIN_SPOKE_COUNT = 12;

    /// <summary>
    /// The maximum number of spokes allowed in a configuration.
    /// </summary>
    public const int MAX_SPOKE_COUNT = 48;

    /// <summary>
    /// The minimum allowable value for a lacing pattern.
    /// </summary>
    public const int MIN_LACING_PATTERN = 0;

    /// <summary>
    /// The maximum number of lacing patterns allowed.
    /// </summary>
    public const int MAX_LACING_PATTERN = 4;

    public SpokeLengthCalculatorValidationBuilder(ValidationContext? validationContext = null) : base(validationContext)
    {

    }

    /// <inheritdoc/>
    protected override void ConfigureValidationRules()
    {
        HasRange(
            nameof(SpokeLengthCalculatorInput.HubFlangeDiameterGearSideMm),
            MIN_HUB_FLANGE_DIAMETER_MM,
            MAX_HUB_FLANGE_DIAMETER_MM,
            "Hub flange diameter for gear side must be between {0} and {1} {2}.",
            "mm");

        HasRange(
            nameof(SpokeLengthCalculatorInput.HubFlangeDiameterNonGearSideMm),
            MIN_HUB_FLANGE_DIAMETER_MM,
            MAX_HUB_FLANGE_DIAMETER_MM,
            "Hub flange diameter for non-gear side must be between {0} and {1} {2}.",
            "mm");

        HasRange(
            nameof(SpokeLengthCalculatorInput.HubCenterToFlangeDistanceGearSideMm),
            MIN_HUB_CENTER_TO_FLANGE_DISTANCE_MM,
            MAX_HUB_CENTER_TO_FLANGE_DISTANCE_MM,
            "Hub center to flange distance for gear side must be between {0} and {1} {2}.",
            "mm");

        HasRange(
            nameof(SpokeLengthCalculatorInput.HubCenterToFlangeDistanceNonGearSideMm),
            MIN_HUB_CENTER_TO_FLANGE_DISTANCE_MM,
            MAX_HUB_CENTER_TO_FLANGE_DISTANCE_MM,
            "Hub center to flange distance for non-gear side must be between {0} and {1} {2}.",
            "mm");

        HasRange(
            nameof(SpokeLengthCalculatorInput.RimInternalDiameterMm),
            MIN_ERD_MM,
            MAX_ERD_MM,
            "Rim internal diameter (ERD) must be between {0} and {1} {2}.",
            "mm");

        HasRange(
            nameof(SpokeLengthCalculatorInput.SpokeCount),
            MIN_SPOKE_COUNT,
            MAX_SPOKE_COUNT,
            "Spoke count must be between {0} and {1}.");

        HasRange(
            nameof(SpokeLengthCalculatorInput.SpokeLacingPattern),
            MIN_LACING_PATTERN,
            MAX_LACING_PATTERN,
            "Lacing pattern must be between {0} and {1}.");
    }
}