namespace velowrench.Core.Models;

/// <summary>
/// Represents a single row of gear calculation results for display purposes.
/// Contains calculated values for a specific sprocket size across all available chainrings.
/// </summary>
public class GearCalculResultRowModel
{
    /// <summary>
    /// Gets or sets the number of teeth on the sprocket for this calculation row.
    /// This value identifies which sprocket size this row represents.
    /// </summary>
    public int SprocketCount { get; set; }

    /// <summary>
    /// Gets or sets the calculated value for the primary (largest or only) chainring.
    /// Always contains a value as the primary chainring is required for all calculations.
    /// </summary>
    public double Chainring1 { get; set; }

    /// <summary>
    /// Gets or sets the calculated value for the medium chainring.
    /// Contains a value only when a medium chainring is configured in the input.
    /// </summary>
    public double? Chainring2 { get; set; }

    /// <summary>
    /// Gets or sets the calculated value for the smallest chainring.
    /// Contains a value only when a small chainring is configured in the input.
    /// </summary>
    public double? Chainring3 { get; set; }
}
