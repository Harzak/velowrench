namespace velowrench.Repository.Models;

/// <summary>
/// Represents a bicycle wheel specification with size information and measurements.
/// </summary>
public record WheelSpecificationModel
{
    /// <summary>
    /// Gets the descriptive label for this wheel specification.
    /// </summary>
    public string Label { get; init; }

    /// <summary>
    /// Gets the Bead Seat Diameter (BSD) in inches.
    /// Value measured where the tire bead sits inside the rim, not at the outer edge of the inflated tyre.
    /// </summary>
    public double BSDin { get; init; }

    /// <summary>
    /// Nominal tyre outer diameters
    /// </summary>
    public double TyreOuterDiameterInInch => this.BSDin + (TyreHeightInInch * 2);

    public double TyreHeightInInch { get; init; }

    public WheelSpecificationModel(string label, double bSDin, double tyreHeightIn)
    {
        this.Label = label;
        this.TyreHeightInInch = tyreHeightIn;
        this.BSDin = bSDin;
    }
}
