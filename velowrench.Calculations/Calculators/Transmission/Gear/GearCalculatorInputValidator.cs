using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Calculations.Enums;
using velowrench.Calculations.Interfaces;

namespace velowrench.Calculations.Calculators.Transmission.Gear;

/// <summary>
/// Provides validation logic for gear calculation input parameters.
/// Validates calculation-specific requirements, component specifications, and value ranges 
/// for different types of gear calculations including gain ratio, gear inches, development, and speed.
/// </summary>
internal sealed class GearCalculatorInputValidator : ICalculatorInputValidation<GearCalculatorInput>
{
    private readonly List<string> _errorMessage;

    /// <summary>
    /// The minimum allowable crank length in millimeters.
    /// </summary>
    internal const int MIN_CRANK_LENGTH_MM = 100;
    
    /// <summary>
    /// The maximum allowable crank length in millimeters.
    /// </summary>
    internal const int MAX_CRANK_LENGTH_MM = 190;
    
    /// <summary>
    /// The minimum allowable wheel diameter in inches.
    /// </summary>
    internal const int MIN_WHEEL_DIAMETER_INCH = 7;
    
    /// <summary>
    /// The maximum allowable wheel diameter in inches.
    /// </summary>
    internal const int MAX_WHEEL_DIAMETER_INCH = 32;
    
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
    internal const int MAX_SPROCKET_TEETH_COUNT = 52;
    
    /// <summary>
    /// The maximum number of sprockets allowed in a single calculation.
    /// </summary>
    internal const int MAX_SPROCKETS_COUNT = 15;
    
    /// <summary>
    /// The minimum allowable revolutions per minute for speed calculations.
    /// </summary>
    internal const int MIN_RPM = 20;
    
    /// <summary>
    /// The maximum allowable revolutions per minute for speed calculations.
    /// </summary>
    internal const int MAX_RPM = 150;

    /// <summary>
    /// Gets a collection of validation error messages from the most recent validation attempt.
    /// </summary>
    public IEnumerable<string> ErrorMessages => _errorMessage;

    public GearCalculatorInputValidator()
    {
        _errorMessage = [];
    }

    /// <summary>
    /// Validates gear calculation input parameters against comprehensive business rules.
    /// Performs calculation-type specific validation and general component specification validation.
    /// </summary>
    /// <returns>
    /// <c>true</c> if all validation rules pass; otherwise, <c>false</c>.
    /// Validation includes: non-null input, calculation-type specific requirements (crank length for gain ratio, RPM for speed),
    /// sprocket count and teeth validation, wheel diameter validation, and chainring teeth validation.
    /// </returns>
    public bool Validate(GearCalculatorInput input)
    {
        if (input == null)
        {
            _errorMessage.Add("Input cannot be null.");
            return false;
        }

        if (input.CalculatorType == EGearCalculatorType.GainRatio)
        {
            if (!input.CrankLengthInMilimeter.HasValue)
            {
                _errorMessage.Add("Crank length is required for Gain Ratio calculations.");

            }
            else if (!CrankLengthIsValid(input.CrankLengthInMilimeter.Value))
            {
                _errorMessage.Add($"Crank length must be between {MIN_CRANK_LENGTH_MM} mm and {MAX_CRANK_LENGTH_MM} mm.");
            }
        }

        if (input.CalculatorType == EGearCalculatorType.Speed)
        {
            if (!input.RevolutionPerMinute.HasValue)
            {
                _errorMessage.Add("Revolution per minute is required for Speed calculations.");
            }
            else if (!RpmIsValid(input.RevolutionPerMinute.Value))
            {
                _errorMessage.Add($"Revolution per minute must be between {MIN_RPM} and {MAX_RPM}.");
            }
        }

        if (input.NumberOfTeethBySprocket.Count < 1)
        {
            _errorMessage.Add("At least one sprocket is required.");
        }
        else if (input.NumberOfTeethBySprocket.Count > MAX_SPROCKETS_COUNT)
        {
            _errorMessage.Add($"Maximum {MAX_SPROCKETS_COUNT} sprockets allowed ({input.NumberOfTeethBySprocket.Count} entered).");
        }

        if (input.NumberOfTeethBySprocket.Any(x => !SprocketTeethCountIsValid(x)))
        {
            _errorMessage.Add($"Sprocket teeth counts must be between {MIN_SPROCKET_TEETH_COUNT} and {MAX_SPROCKET_TEETH_COUNT}.");
        }

        if (!WheelDiameterIsValid(input.WheelDiameterInInch))
        {
            _errorMessage.Add($"Wheel diameter must be between {MIN_WHEEL_DIAMETER_INCH} and {MAX_WHEEL_DIAMETER_INCH} inches.");
        }

        if (!ChainringTeethCountIsValid(input.TeethNumberLargeOrUniqueChainring))
        {
            _errorMessage.Add($"Chainring teeth count must be between {MIN_CHAINRING_TEETH_COUNT} and {MAX_CHAINRING_TEETH_COUNT}.");
        }

        if (input.TeethNumberMediumChainring.HasValue && !ChainringTeethCountIsValid(input.TeethNumberMediumChainring.Value))
        {
            _errorMessage.Add($"Chainring teeth count must be between {MIN_CHAINRING_TEETH_COUNT} and {MAX_CHAINRING_TEETH_COUNT}.");
        }

        if (input.TeethNumberSmallChainring.HasValue && !ChainringTeethCountIsValid(input.TeethNumberSmallChainring.Value))
        {
            _errorMessage.Add($"Chainring teeth count must be between {MIN_CHAINRING_TEETH_COUNT} and {MAX_CHAINRING_TEETH_COUNT}.");
        }

        return _errorMessage.Count == 0;
    }

    private bool CrankLengthIsValid(double crankLength)
    {
        return crankLength >= MIN_CRANK_LENGTH_MM && crankLength <= MAX_CRANK_LENGTH_MM;
    }

    private bool WheelDiameterIsValid(double wheelDiameterInch)
    {
        return wheelDiameterInch >= MIN_WHEEL_DIAMETER_INCH && wheelDiameterInch <= MAX_WHEEL_DIAMETER_INCH;
    }

    private bool ChainringTeethCountIsValid(int teethCount)
    {
        return teethCount >= MIN_CHAINRING_TEETH_COUNT && teethCount <= MAX_CHAINRING_TEETH_COUNT;
    }

    private bool SprocketTeethCountIsValid(int teethCount)
    {
        return teethCount >= MIN_SPROCKET_TEETH_COUNT && teethCount <= MAX_SPROCKET_TEETH_COUNT;
    }

    private bool RpmIsValid(int rpm)
    {
        return rpm >= MIN_RPM && rpm <= MAX_RPM;
    }
}

