using velowrench.Calculations.Constants;

namespace velowrench.Calculations.Calculators.Transmission.Chain;

/// <summary>
/// Represents the input parameters required for chain length calculations.
/// </summary>
public sealed class ChainLengthCalculatorInput : BaseCalculatorInput, IEquatable<ChainLengthCalculatorInput>
{
    /// <summary>
    /// Gets the chainstay length in inches.
    /// which is the horizontal distance between the bottom bracket and rear axle.
    /// </summary>
    public double ChainStayLengthIn { get; set; }

    /// <summary>
    /// Gets the number of teeth on the largest chainring (front gear).
    /// </summary>
    public int TeethLargestChainring { get; set; }

    /// <summary>
    /// Gets the number of teeth on the largest sprocket (rear gear).
    /// </summary>
    public int TeethLargestSprocket { get; set; }


    public ChainLengthCalculatorInput() : base()
    {

    }

    public ChainLengthCalculatorInput(int precision) : base(precision)
    {

    }

    internal ChainLengthCalculatorInput Copy()
    {
        return new ChainLengthCalculatorInput(base.Precision)
        {
            ChainStayLengthIn = this.ChainStayLengthIn,
            TeethLargestChainring = this.TeethLargestChainring,
            TeethLargestSprocket = this.TeethLargestSprocket
        };
    }

    public bool Equals(ChainLengthCalculatorInput? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;

        const double tolerance = 1e-10;
        return Math.Abs(ChainStayLengthIn - other.ChainStayLengthIn) < tolerance
            && TeethLargestChainring == other.TeethLargestChainring
            && TeethLargestSprocket == other.TeethLargestSprocket;
    }

    public override bool Equals(object? obj) => Equals(obj as ChainLengthCalculatorInput);

    public override int GetHashCode()
    {
        double roundedLength = Math.Round(ChainStayLengthIn, 10);
        return HashCode.Combine(roundedLength, TeethLargestChainring, TeethLargestSprocket);
    }
}