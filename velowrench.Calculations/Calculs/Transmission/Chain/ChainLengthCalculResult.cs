namespace velowrench.Calculations.Calculs.Transmission.Chain;

/// <summary>
/// Represents the result of a chain length calculation for bicycle transmissions.
/// </summary>
public record ChainLengthCalculResult
{
    /// <summary>
    /// Gets the number of chain links required for the calculated chain length.
    /// </summary>
    public required int ChainLinks { get; init; }
    
    /// <summary>
    /// Gets the calculated chain length in inches.
    /// </summary>
    public required double ChainLengthInch { get; init; }
    
    /// <summary>
    /// Gets the UTC timestamp when this calculation was performed.
    /// </summary>
    public required DateTime CalculatedAt { get; init; }
    
    /// <summary>
    /// Gets the input parameters that were used to produce this calculation result.
    /// </summary>
    public required ChainLengthCalculInput UsedInputs { get; init; }
}