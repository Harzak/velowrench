namespace velowrench.Calculations.Calculs.Transmission.Chain;

/// <summary>
/// Represents the input parameters required for chain length calculations.
/// </summary>
public record ChainLengthCalculInput
{
    /// <summary>
    /// Gets the chainstay length in inches, which is the horizontal distance between the bottom bracket and rear axle.
    /// </summary>
    public required double ChainStayLengthInch { get; init; }
    
    /// <summary>
    /// Gets the number of teeth on the largest chainring (front gear).
    /// </summary>
    public required int TeethLargestChainring { get; init; }
    
    /// <summary>
    /// Gets the number of teeth on the largest sprocket (rear gear).
    /// </summary>
    public required int TeethLargestSprocket { get; init; }

    /// <summary>
    /// Determines whether the specified <see cref="ChainLengthCalculInput"/> is equal to the current instance.
    /// </summary>
    /// <remarks>
    /// Equality comparison uses a tolerance of 1e-10 for floating-point chainstay length comparison
    /// to account for potential precision issues.
    /// </remarks>
    public virtual bool Equals(ChainLengthCalculInput? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        const double tolerance = 1e-10;
        return Math.Abs(ChainStayLengthInch - other.ChainStayLengthInch) < tolerance 
            && TeethLargestChainring == other.TeethLargestChainring
            && TeethLargestSprocket == other.TeethLargestSprocket;
    }

    /// <summary>
    /// Returns a hash code for this instance, suitable for use in hash tables and dictionaries.
    /// </summary>
    /// <remarks>
    /// The hash code is computed based on the rounded chainstay length and the exact tooth counts
    /// to ensure consistency with the equality comparison.
    /// </remarks>
    public override int GetHashCode()
    {
        double roundedLength = Math.Round(ChainStayLengthInch, 10);
        return HashCode.Combine(roundedLength, TeethLargestChainring, TeethLargestSprocket);
    }
}