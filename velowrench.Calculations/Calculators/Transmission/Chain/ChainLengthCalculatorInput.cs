namespace velowrench.Calculations.Calculators.Transmission.Chain;

/// <summary>
/// Represents the input parameters required for chain length calculations.
/// </summary>
public sealed record ChainLengthCalculatorInput
{
    /// <summary>
    /// Gets the chainstay length in inches.
    /// which is the horizontal distance between the bottom bracket and rear axle.
    /// </summary>
    public required double ChainStayLengthIn { get; init; }

    /// <summary>
    /// Gets the number of teeth on the largest chainring (front gear).
    /// </summary>
    public required int TeethLargestChainring { get; init; }

    /// <summary>
    /// Gets the number of teeth on the largest sprocket (rear gear).
    /// </summary>
    public required int TeethLargestSprocket { get; init; }

    /// <summary>
    /// Custom equality implementation with floating-point tolerance.
    /// </summary>
    public bool Equals(ChainLengthCalculatorInput? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;

        const double tolerance = 1e-10;
        return Math.Abs(ChainStayLengthIn - other.ChainStayLengthIn) < tolerance
            && TeethLargestChainring == other.TeethLargestChainring
            && TeethLargestSprocket == other.TeethLargestSprocket;
    }

    public override int GetHashCode()
    {
        double roundedLength = Math.Round(ChainStayLengthIn, 10);
        return HashCode.Combine(roundedLength, TeethLargestChainring, TeethLargestSprocket);
    }
}