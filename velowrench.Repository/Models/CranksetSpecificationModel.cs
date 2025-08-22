namespace velowrench.Repository.Models;

/// <summary>
/// Represents a bicycle crankset specification with crank arm length information.
/// </summary>
public sealed record CranksetSpecificationModel
{
    /// <summary>
    /// Gets the descriptive label for this crankset specification.
    /// </summary>
    public string Label { get; init; }

    /// <summary>
    /// Gets the crank arm length measurement.
    /// </summary>
    public double Length { get; init; }

    public CranksetSpecificationModel(string label, double length)
    {
        this.Label = label;
        this.Length = length;
    }
}
