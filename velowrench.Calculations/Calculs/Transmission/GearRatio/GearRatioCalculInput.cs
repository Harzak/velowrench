using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace velowrench.Calculations.Calculs.Transmission.GearRatio;

/// <summary>
/// Represents the input parameters required for gear ratio calculations.
/// </summary>
public abstract record GearRatioCalculInput
{
    public required int TeethNumberLargeOrUniqueChainring { get; init; }
    public int? TeethNumberMediumChainring { get; init; }
    public int? TeethNumberSmallChainring { get; init; }
    public required IList<int> NumberOfTeethBySprocket { get; init; }
    public int Precision { get; init; }  
    public required double WheelDiameterInInch { get; init; }

    protected GearRatioCalculInput()
    {
        this.Precision = 1;
    }
}
