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
public record GearRatioCalculInput
{
    public ReadOnlyCollection<int> NumberOfTeethByChainring { get; init; }
    public ReadOnlyCollection<int> NumberOfTeethBySprocket { get; init; }

    public GearRatioCalculInput(IList<int> numberOfTeethByChainring, IList<int> numberOfTeethBySprocket)
    {
        this.NumberOfTeethByChainring = new ReadOnlyCollection<int>(numberOfTeethByChainring);
        this.NumberOfTeethBySprocket = new ReadOnlyCollection<int>(numberOfTeethBySprocket);
    }
}
