using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitsNet;
using UnitsNet.Units;
using velowrench.Calculations.Enums;
using velowrench.Calculations.Units;

namespace velowrench.Calculations.Calculators.Transmission.Gear;

/// <summary>
/// Represents the input parameters required for gear ratio calculations.
/// </summary>
public sealed record GearCalculatorInput
{
    /// <summary>
    /// Gets the number of teeth on the largest or unique chainring (front gear).
    /// This is the primary chainring used in single chainring setups or the largest ring in multi-ring configurations.
    /// </summary>
    public required int TeethNumberLargeOrUniqueChainring { get; init; }
    
    /// <summary>
    /// Gets the number of teeth on the medium chainring (front gear).
    /// Optional parameter used in triple chainring configurations.
    /// </summary>
    public int? TeethNumberMediumChainring { get; init; }
    
    /// <summary>
    /// Gets the number of teeth on the smallest chainring (front gear).
    /// Optional parameter used in double or triple chainring configurations.
    /// </summary>
    public int? TeethNumberSmallChainring { get; init; }
    
    /// <summary>
    /// Gets the collection of teeth counts for all sprockets in the cassette or freewheel (rear gears).
    /// Each value represents the number of teeth on a specific sprocket.
    /// </summary>
    public required IList<int> NumberOfTeethBySprocket { get; init; }

    /// <summary>
    /// Gets the outer tyre diameter in a specified unit (e.g., inches, millimeters).
    /// This measurement affects the actual distance traveled per wheel revolution.
    /// </summary>
    public required ConvertibleDouble<LengthUnit> TyreOuterDiameter { get; init; }

    /// <summary>
    /// Gets the crank arm length in a specified unit (e.g., millimeters).
    /// Only used for gain ratio calculations, should be set to null for other calculation types.
    /// </summary>
    /// <value>
    /// Required for <see cref="EGearCalculatorType.GainRatio"/> calculations, null for other calculation types
    /// </value>
    public required ConvertibleDouble<LengthUnit>? CrankLength { get; init; }

    /// <summary>
    /// Gets the pedaling cadence in revolutions per minute.
    /// Only used for speed calculations, should be set to null for other calculation types.
    /// </summary>
    /// <value>
    /// Required for <see cref="EGearCalculatorType.Speed"/> calculations, null for other calculation types
    /// </value>
    public required int? RevolutionPerMinute { get; init; }

    /// <summary>
    /// Gets the type of gear calculation to perform.
    /// Determines which calculation algorithm is used and which optional parameters are required.
    /// </summary>
    public required EGearCalculatorType CalculatorType { get; init; }
    
    /// <summary>
    /// Gets the number of decimal places to include in calculation results.
    /// Controls the precision of the output values.
    /// </summary>
    public int Precision { get; init; }

    public GearCalculatorInput()
    {
        this.Precision = 1;
    }
}
