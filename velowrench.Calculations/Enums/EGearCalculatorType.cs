using System.ComponentModel.DataAnnotations;

namespace velowrench.Calculations.Enums;

/// <summary>
/// Defines the types of gear calculations that can be performed.
/// Each calculation type produces different metrics for analyzing bicycle gear performance.
/// </summary>
public enum EGearCalculatorType
{
    /// <summary>
    /// Traditional gear inches calculation that represents the diameter of a wheel 
    /// that would travel the same distance per pedal revolution as the actual gear combination.
    /// Standard measurement for comparing gear ratios across different bicycle configurations.
    /// </summary>
    [Display(Name = "Gear Inches")]
    GearInches = 0,

    /// <summary>
    /// Gain ratio calculation that considers crank arm length in the gear ratio formula.
    /// Provides a more accurate representation of mechanical advantage for pedaling efficiency.
    /// </summary>
    [Display(Name = "Gain Ratio")]
    GainRatio = 1,

    /// <summary>
    /// Development calculation that measures the distance traveled per pedal revolution.
    /// This metric directly shows how far the bicycle moves forward 
    /// with each complete pedal stroke for a given gear combination.
    /// </summary>
    [Display(Name = "Development")]
    Development = 2,

    /// <summary>
    /// Speed calculation that determines theoretical speed based on cadence and gear ratio.
    /// Calculates the speed achievable at a specific pedaling rate (RPM) for each gear combination.
    /// </summary>
    [Display(Name = "Speed")]
    Speed = 3,
}
