namespace velowrench.Calculations.Calculs.Transmission.ChainLength;

public record ChainLengthCalculInput
{
    public double ChainStayLengthInch { get; init; }
    public int TeethLargestChainring { get; init; }
    public int TeethLargestSprocket { get; init; }

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

    public override int GetHashCode()
    {
        double roundedLength = Math.Round(ChainStayLengthInch, 10);
        return HashCode.Combine(roundedLength, TeethLargestChainring, TeethLargestSprocket);
    }
}