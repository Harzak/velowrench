namespace velowrench.Repository.Models;

/// <summary>
/// Represents a cadence specification for cycling performance calculations.
/// </summary>
public sealed record CadenceModel
{
    /// <summary>
    /// Gets the descriptive label for this cadence specification.
    /// </summary>
    public string Label { get; init; }

    /// <summary>
    /// Gets the cadence value in revolutions per minute.
    /// </summary>
    public int Rpm { get; init; }

    public CadenceModel(string label, int rpm)
    {
        this.Label = label;
        this.Rpm = rpm;
    }
}
