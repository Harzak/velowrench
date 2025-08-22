namespace velowrench.Repository.Models;

/// <summary>
/// Represents a spoke lacing pattern, including its descriptive label and the number of crosses.
/// </summary>
public sealed record SpokeLacingPatternModel
{
    /// <summary>
    /// Gets the descriptive label for this spoke lacing pattern.
    /// </summary>
    public string Label { get; init; }

    /// <summary>
    /// Gets the number of crosses in this spoke lacing pattern.
    /// </summary>
    public int Crosses { get; init; }

    public SpokeLacingPatternModel(string label, int crosses)
    {
        this.Label = label;
        this.Crosses = crosses;
    }
}

