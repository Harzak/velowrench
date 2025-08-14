using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace velowrench.Calculations.Constants;

public static class ChainConst
{
    /// <summary>
    /// Industry Standard for bicycle chain link length, in inches.
    /// </summary>
    public const double CHAINLINK_LENGTH_INCH = 0.5;

    /// <summary>
    /// The total straight-line distance along both top and bottom chain runs.
    /// </summary>
    public const double CHAINSTAY_MULTIPLIER = 2.0;

    /// <summary>
    /// An approximation of the chain length added by gear wrap — since the chain wraps partially around both gears, not fully.
    /// </summary>
    public const double TEETH_WRAP_RATIO = 0.25;
}