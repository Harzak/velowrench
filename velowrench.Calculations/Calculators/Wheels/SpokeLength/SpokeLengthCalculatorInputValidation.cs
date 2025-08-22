using velowrench.Calculations.Interfaces;

namespace velowrench.Calculations.Calculators.Wheels.SpokeLength;

/// <summary>
/// Provides validation logic for <see cref="SpokeLengthCalculatorInput"/> instances, ensuring that all input values
/// fall within acceptable ranges for spoke length calculations.
/// </summary>
internal sealed class SpokeLengthCalculatorInputValidation : ICalculatorInputValidation<SpokeLengthCalculatorInput>
{
    private readonly List<string> _errorMessage;

    /// <summary>
    /// The minimum allowable diameter, in millimeters, for a hub flange.
    /// </summary>
    private const int MIN_HUB_FLANGE_DIAMETER_MM = 20;

    /// <summary>
    /// The maximum allowable diameter, in millimeters, for a hub flange.
    /// </summary>
    private const int MAX_HUB_FLANGE_DIAMETER_MM = 80;

    /// <summary>
    /// The minimum allowable distance, in millimeters, between the hub center and the flange.
    /// </summary>
    private const int MIN_HUB_CENTER_TO_FLANGE_DISTANCE_MM = 10;

    /// <summary>
    /// The maximum allowable distance, in millimeters, between the hub center and the flange.
    /// </summary>
    private const int MAX_HUB_CENTER_TO_FLANGE_DISTANCE_MM = 60;

    /// <summary>
    /// The minimum Effective Rim Diameter (ERD) in millimeters.
    /// </summary>
    private const int MIN_ERD_MM = 200;

    /// <summary>
    /// The maximum allowable size, in millimeters, for the ERD (Effective Radiated Diameter).
    /// </summary>
    private const int MAX_ERD_MM = 800;

    /// <summary>
    /// The minimum number of spokes required for a valid configuration.
    /// </summary>
    private const int MIN_SPOKE_COUNT = 12;

    /// <summary>
    /// The maximum number of spokes allowed in a configuration.
    /// </summary>
    private const int MAX_SPOKE_COUNT = 48;

    /// <summary>
    /// The minimum allowable value for a lacing pattern.
    /// </summary>
    private const int MIN_LACING_PATTERN = 0;

    /// <summary>
    /// The maximum number of lacing patterns allowed.
    /// </summary>
    private const int MAX_LACING_PATTERN = 4;

    public IEnumerable<string> ErrorMessages => _errorMessage;

    public SpokeLengthCalculatorInputValidation()
    {
        _errorMessage = [];
    }

    /// <summary>
    /// Validates the provided input for calculating spoke lengths.
    /// </summary>
    public bool Validate(SpokeLengthCalculatorInput input)
    {
        if (input == null)
        {
            _errorMessage.Add("Input cannot be null.");
            return false;
        }

        if (!HubFlangeDiameterIsValid(input.HubFlangeDiameterGearSideMm)
           || !HubFlangeDiameterIsValid(input.HubFlangeDiameterNonGearSideMm))
        {
            _errorMessage.Add($"Hub flange diameter must be between {MIN_HUB_FLANGE_DIAMETER_MM} mm and {MAX_HUB_FLANGE_DIAMETER_MM} mm.");
        }

        if (!HubCenterToFlangeDistanceIsValid(input.HubCenterToFlangeDistanceGearSideMm)
            || !HubCenterToFlangeDistanceIsValid(input.HubCenterToFlangeDistanceGearSideMm))
        {
            _errorMessage.Add($"Hub center to flange distance must be between {MIN_HUB_CENTER_TO_FLANGE_DISTANCE_MM} mm and {MAX_HUB_CENTER_TO_FLANGE_DISTANCE_MM} mm.");
        }

        if (!ERDIsValid(input.RimInternalDiameterMm))
        {
            _errorMessage.Add($"Rim internal diameter (ERD) must be between {MIN_ERD_MM} mm and {MAX_ERD_MM} mm.");
        }

        if (!SpokeCountIsValid(input.SpokeCount))
        {
            _errorMessage.Add($"Spoke count must be between {MIN_SPOKE_COUNT} and {MAX_SPOKE_COUNT}.");
        }

        if (!LacingPatternIsValid(input.SpokeLacingPattern))
        {
            _errorMessage.Add($"Lacing pattern must be between {MIN_LACING_PATTERN} and {MAX_LACING_PATTERN}.");
        }

        return _errorMessage.Count == 0;
    }

    private bool HubFlangeDiameterIsValid(double hubFlangeDiameterMm)
    {
        return hubFlangeDiameterMm >= MIN_HUB_FLANGE_DIAMETER_MM && hubFlangeDiameterMm <= MAX_HUB_FLANGE_DIAMETER_MM;
    }

    private bool HubCenterToFlangeDistanceIsValid(double hubCenterToFlangeDistanceMm)
    {
        return hubCenterToFlangeDistanceMm >= MIN_HUB_CENTER_TO_FLANGE_DISTANCE_MM && hubCenterToFlangeDistanceMm <= MAX_HUB_CENTER_TO_FLANGE_DISTANCE_MM;
    }

    private bool ERDIsValid(double erdMm)
    {
        return erdMm >= MIN_ERD_MM && erdMm <= MAX_ERD_MM;
    }

    private bool SpokeCountIsValid(int spokeCount)
    {
        return spokeCount >= MIN_SPOKE_COUNT && spokeCount <= MAX_SPOKE_COUNT;
    }

    private bool LacingPatternIsValid(int lacingPattern)
    {
        return lacingPattern >= MIN_LACING_PATTERN && lacingPattern <= MAX_LACING_PATTERN;
    }

}
