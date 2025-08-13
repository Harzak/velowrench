namespace velowrench.Calculations.Calculators.Transmission.Gear;

/// <summary>
/// Represents the result of gear ratio calculations.
/// Contains calculated values for each sprocket and chainring combination,
/// organized by chainring size for easy comparison and analysis.
/// </summary>
public sealed record GearCalculatorResult : BaseCalculatorResult<GearCalculatorInput>
{
    /// <summary>
    /// Gets the calculated values for the large or unique chainring paired with each sprocket.
    /// Contains one value for each sprocket in the input, calculated using the largest or only chainring.
    /// Values represent the calculated metric (gear inches, development, gain ratio, or speed) depending on the calculation type.
    /// </summary>
    public required IList<double> ValuesLargeOrUniqueChainring { get; init; }
    
    /// <summary>
    /// Gets the calculated values for the medium chainring paired with each sprocket.
    /// Only populated when a medium chainring was specified in the input parameters.
    /// Contains one value for each sprocket, calculated using the medium chainring.
    /// </summary>
    public IList<double>? ValuesMediumChainring { get; init; }
    
    /// <summary>
    /// Gets the calculated values for the small chainring paired with each sprocket.
    /// Only populated when a small chainring was specified in the input parameters.
    /// Contains one value for each sprocket, calculated using the small chainring.
    /// </summary>
    public IList<double>? ValuesSmallChainring { get; init; }

    public GearCalculatorResult()
    {

    }
}
