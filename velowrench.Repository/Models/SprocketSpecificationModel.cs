namespace velowrench.Repository.Models;

/// <summary>
/// Represents a bicycle sprocket specification with teeth count information.
/// </summary>
public record SprocketSpecificationModel
{
    /// <summary>
    /// Gets the descriptive label for this sprocket specification.
    /// </summary>
    public string Label { get; init; }

    /// <summary>
    /// Gets the number of teeth on this sprocket.
    /// </summary>
    public int TeethCount { get; init; }

    public SprocketSpecificationModel(string label, int teethCount)
    {
        Label = label;
        TeethCount = teethCount;
    }
}
