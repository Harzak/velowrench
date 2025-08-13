using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using velowrench.Calculations.Interfaces;

[assembly: InternalsVisibleTo("velowrench.Calculations.Tests")]

namespace velowrench.Calculations.Calculs.Transmission.Chain;

internal sealed class ChainLengthCalculInputValidation : ICalculInputValidation<ChainLengthCalculInput>
{
    internal const double MIN_CHAINSTAY_LENGTH_INCH = 1.0;
    internal const int MIN_CHAINRING_TEETH_COUNT = 10;
    internal const int MAX_CHAINRING_TEETH_COUNT = 120;
    internal const int MIN_SPROCKET_TEETH_COUNT = 9;
    internal const int MAX_SPROCKET_TEETH_COUNT = 52;

    private readonly List<string> _errorMessage;

    public IEnumerable<string> ErrorMessages => _errorMessage;

    public ChainLengthCalculInputValidation()
    {
        _errorMessage = [];
    }
    public bool Validate(ChainLengthCalculInput input)
    {
        if (input == null)
        {
            _errorMessage.Add("Input cannot be null.");
            return false;
        }

        if(!ChainStayLengthIsValid(input.ChainStayLengthInch))
        {
            _errorMessage.Add($"Chainstay length must be greater than {MIN_CHAINSTAY_LENGTH_INCH}.");
        }

        if (!ChainringTeethCountIsValid(input.TeethLargestChainring))
        {
            _errorMessage.Add($"Chainring teeth count must be between {MIN_CHAINRING_TEETH_COUNT} and {MAX_CHAINRING_TEETH_COUNT}.");
        }

        if (!SprocketTeethCountIsValid(input.TeethLargestSprocket))
        {
            _errorMessage.Add($"Sprocket teeth count must be between {MIN_SPROCKET_TEETH_COUNT} and {MAX_SPROCKET_TEETH_COUNT}.");
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
