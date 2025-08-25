using System.Collections.ObjectModel;
using velowrench.Calculations.Constants;
using velowrench.Calculations.Enums;

namespace velowrench.Calculations.Calculators.Transmission.Gear;

/// <summary>
/// Represents the input parameters required for gear ratio calculations.
/// </summary>
public sealed class GearCalculatorInput: IEquatable<GearCalculatorInput>
{
    /// <summary>
    /// Gets the number of teeth on the largest or unique chainring (front gear).
    /// This is the primary chainring used in single chainring setups or the largest ring in multi-ring configurations.
    /// </summary>
    public int TeethNumberLargeOrUniqueChainring { get; set; }

    /// <summary>
    /// Gets the number of teeth on the medium chainring (front gear).
    /// Optional parameter used in triple chainring configurations.
    /// </summary>
    public int? TeethNumberMediumChainring { get; set; }

    /// <summary>
    /// Gets the number of teeth on the smallest chainring (front gear).
    /// Optional parameter used in double or triple chainring configurations.
    /// </summary>
    public int? TeethNumberSmallChainring { get; set; }

    /// <summary>
    /// Gets the collection of teeth counts for all sprockets in the cassette or freewheel (rear gears).
    /// Each value represents the number of teeth on a specific sprocket.
    /// </summary>
    public IReadOnlyList<int> NumberOfTeethBySprocket { get; private set; }

    /// <summary>
    /// Gets the outer tyre diameter in inches.
    /// This measurement affects the actual distance traveled per wheel revolution.
    /// </summary>
    public double TyreOuterDiameterIn { get; set; }

    /// <summary>
    /// Gets the crank arm length millimeters.
    /// Only used for gain ratio calculations, should be set to null for other calculation types.
    /// </summary>
    /// <value>
    /// Required for <see cref="EGearCalculatorType.GainRatio"/> calculations, null for other calculation types
    /// </value>
    public double CrankLengthMm { get; set; }

    /// <summary>
    /// Gets the pedaling cadence in revolutions per minute.
    /// Only used for speed calculations, should be set to null for other calculation types.
    /// </summary>
    /// <value>
    /// Required for <see cref="EGearCalculatorType.Speed"/> calculations, null for other calculation types
    /// </value>
    public int? RevolutionPerMinute { get; set; }

    /// <summary>
    /// Gets the type of gear calculation to perform.
    /// Determines which calculation algorithm is used and which optional parameters are required.
    /// </summary>
    public EGearCalculatorType CalculatorType { get; set; }

    /// <summary>
    /// Gets the number of decimal places to include in calculation results.
    /// Controls the precision of the output values.
    /// </summary>
    public int Precision { get; set; }

    public GearCalculatorInput(int precision = CalculationConstants.DEFAULT_PRECISION)
    {
        this.Precision = precision;
        this.NumberOfTeethBySprocket = [];
    }

    public GearCalculatorInput(IEnumerable<int> sprockets, int precision = CalculationConstants.DEFAULT_PRECISION)
    {
        this.Precision = precision;
        this.NumberOfTeethBySprocket = sprockets.ToList();
    }

    public void WithSprockets(IEnumerable<int> sprockets)
    {
        NumberOfTeethBySprocket = sprockets.ToList();
    }

    internal GearCalculatorInput Copy()
    {
        return new GearCalculatorInput(this.Precision)
        {
            CalculatorType = this.CalculatorType,
            CrankLengthMm = this.CrankLengthMm,
            RevolutionPerMinute = this.RevolutionPerMinute,
            TeethNumberLargeOrUniqueChainring = this.TeethNumberLargeOrUniqueChainring,
            TeethNumberMediumChainring = this.TeethNumberMediumChainring,
            TeethNumberSmallChainring = this.TeethNumberSmallChainring,
            TyreOuterDiameterIn = this.TyreOuterDiameterIn,
            NumberOfTeethBySprocket = this.NumberOfTeethBySprocket.ToList()
        };
    }

    public bool Equals(GearCalculatorInput? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;

        const double tolerance = 1e-10;
        return TeethNumberLargeOrUniqueChainring == other.TeethNumberLargeOrUniqueChainring
            && TeethNumberMediumChainring == other.TeethNumberMediumChainring
            && TeethNumberSmallChainring == other.TeethNumberSmallChainring
            && Math.Abs(TyreOuterDiameterIn - other.TyreOuterDiameterIn) < tolerance
            && Math.Abs(CrankLengthMm - other.CrankLengthMm) < tolerance
            && RevolutionPerMinute == other.RevolutionPerMinute
            && CalculatorType == other.CalculatorType
            && Precision == other.Precision
            && NumberOfTeethBySprocket.SequenceEqual(other.NumberOfTeethBySprocket);
    }

    public override bool Equals(object? obj) => Equals(obj as GearCalculatorInput);

    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(TeethNumberLargeOrUniqueChainring);
        hash.Add(TeethNumberMediumChainring);
        hash.Add(TeethNumberSmallChainring);
        hash.Add(TyreOuterDiameterIn);
        hash.Add(CrankLengthMm);
        hash.Add(RevolutionPerMinute);
        hash.Add(CalculatorType);
        hash.Add(Precision);
        foreach (var tooth in NumberOfTeethBySprocket)
            hash.Add(tooth);
        return hash.ToHashCode();
    }
}
