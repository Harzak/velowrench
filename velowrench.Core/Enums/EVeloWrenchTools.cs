namespace velowrench.Core.Enums;

/// <summary>
/// Enumeration of available bicycle maintenance and calculation tools in the VeloWrench application.
/// </summary>
public enum EVeloWrenchTools
{
    /// <summary>
    /// Unknown or unspecified tool type.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// Tool for calculating optimal bicycle chain length based on drivetrain specifications.
    /// </summary>
    ChainLengthCalculator = 1,

    /// <summary>
    /// Tool for calculating gain ratios/ gear inches/ development/ speed.
    /// </summary>
    GearCalculator = 2,

    /// <summary>
    /// Tool for calculating proper spoke length for wheel building.
    /// </summary>
    SpokeLengthCalculator = 3,

    /// <summary>
    /// Guide for proper spoke tension measurements and adjustments.
    /// </summary>
    SpokeTensionGuide = 4,

    /// <summary>
    /// Guide for checking compatibility between different bicycle components.
    /// </summary>
    ComponentCompatibilityGuide = 5,

    /// <summary>
    /// Tool for checking axle and hub standard compatibility.
    /// </summary>
    AxleAndHubStandardChecker = 6
}
