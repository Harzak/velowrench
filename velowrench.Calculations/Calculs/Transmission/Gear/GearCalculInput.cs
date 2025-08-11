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
public abstract record GearCalculInput
{
    public required int TeethNumberLargeOrUniqueChainring { get; init; }
    public int? TeethNumberMediumChainring { get; init; }
    public int? TeethNumberSmallChainring { get; init; }
    public required IList<int> NumberOfTeethBySprocket { get; init; }
    public int Precision { get; init; }  
    public required double WheelDiameterInInch { get; init; }

    protected GearCalculInput()
    {
        this.Precision = 1;
    }
}
