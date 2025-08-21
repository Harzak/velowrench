using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitsNet.Units;
using velowrench.Calculations.Units;

namespace velowrench.Calculations.Calculators.Wheels.SpokeLength;

public record SpokeLengthCalculatorInput
{
    public required ConvertibleDouble<LengthUnit> HubCenterToFlangeGearSideDistance { get; init; }
    public required ConvertibleDouble<LengthUnit> HubCenterToFlangeNonGearSideDistance { get; init; }
    public required ConvertibleDouble<LengthUnit> HubFlangeDiameterGearSide { get; init; }
    public required ConvertibleDouble<LengthUnit> HubFlangeDiameterNonGearSide { get; init; }
    public required ConvertibleDouble<LengthUnit> RimInternalDiameter { get; init; }
    public required int SpokeCount { get; init; }
    public required int SpokeLacingPattern { get; init; }
}

