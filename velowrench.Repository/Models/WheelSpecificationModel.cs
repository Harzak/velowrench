using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    /// Gets the Bead Seat Diameter (BSD) in millimeters.
    /// This is the standardized measurement used in wheel manufacturing and tire compatibility.
    /// </summary>
    public int BSDmm { get; init; }
    
    /// <summary>
    /// Gets the Bead Seat Diameter (BSD) in inches.
    /// This measurement is commonly used in gear calculations and imperial-based formulas.
    /// </summary>
    public double BSDin { get; init; }

    public WheelSpecificationModel(string label, double bSDin, int bSDmm)
    {
        this.Label = label;
        this.BSDmm = bSDmm;
        this.BSDin = bSDin;
    }
}
