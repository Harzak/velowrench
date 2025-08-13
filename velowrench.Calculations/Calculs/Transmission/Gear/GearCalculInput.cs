using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace velowrench.Calculations.Calculs.Transmission.Gear;

/// <summary>
/// Represents the input parameters required for gear ratio calculations.
/// </summary>
public sealed record GearCalculInput
{
    public required int TeethNumberLargeOrUniqueChainring { get; init; }
    public int? TeethNumberMediumChainring { get; init; }
    public int? TeethNumberSmallChainring { get; init; }
    public required IList<int> NumberOfTeethBySprocket { get; init; }
    public required double WheelDiameterInInch { get; init; }
    /// <summary>
    /// Only used for gain ratio calculations, set at null otherwise.
    /// </summary>
    public required double? CrankLengthInInch { get; init; }

    /// <summary>
    /// Only used for speed calculations, set at null otherwise.
    /// </summary>
    public required int? RevolutionPerMinute { get; init; }

    public required EGearCalculType CalculType { get; init; }
    public int Precision { get; init; }  

    public GearCalculInput()
    {
        this.Precision = 1;
    }
}
