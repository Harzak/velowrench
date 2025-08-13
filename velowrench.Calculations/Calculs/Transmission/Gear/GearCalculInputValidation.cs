using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Calculations.Interfaces;

namespace velowrench.Calculations.Calculs.Transmission.Gear;

internal sealed class GearCalculInputValidation: ICalculInputValidation<GearCalculInput>
{
    internal const int MIN_CRANK_LENGTH_MM = 100;
    internal const int MAX_CRANK_LENGTH_MM = 190;
    internal const int MIN_WHEEL_DIAMETER_INCH = 7;
    internal const int MAX_WHEEL_DIAMETER_INCH = 32;
    internal const int MIN_CHAINRING_TEETH_COUNT = 10;
    internal const int MAX_CHAINRING_TEETH_COUNT = 120;
    internal const int MIN_SPROCKET_TEETH_COUNT = 9;
    internal const int MAX_SPROCKET_TEETH_COUNT = 52;
    internal const int MAX_SPROCKETS_COUNT = 15;
    internal const int MIN_RPM = 20;
    internal const int MAX_RPM = 150;

    public IEnumerable<string> ErrorMessages => _errorMessage;

    private readonly List<string> _errorMessage;

    public GearCalculInputValidation()
    {
        _errorMessage = [];
    }

    public bool Validate(GearCalculInput input)
    {
        List<string> errorLst = [];

        if (input == null)
        {
            _errorMessage.Add("Input cannot be null.");
            return false;
        }

        if (input.CalculType == EGearCalculType.GainRatio)
        {
            if (!input.CrankLengthInInch.HasValue)
            {
                _errorMessage.Add("Crank length is required for Gain Ratio calculations.");

            }
            else if (!CrankLengthIsValid(input.CrankLengthInInch.Value))
            {
                _errorMessage.Add($"Crank length must be between {MIN_CRANK_LENGTH_MM} mm and {MAX_CRANK_LENGTH_MM} mm.");
            }
        }

        if (input.CalculType == EGearCalculType.Speed)
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

        if(input.NumberOfTeethBySprocket.Any(x => !SprocketTeethCountIsValid(x)))
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

