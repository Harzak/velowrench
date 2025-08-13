namespace velowrench.Calculations.Calculs.Transmission.Gear;

/// <summary>
/// Represents the result of a gear ratio calculations.
/// </summary>
public sealed record GearCalculResult
{
    public required IList<double> ValuesLargeOrUniqueChainring { get; init; }
    public IList<double>? ValuesMediumChainring { get; init; }
    public IList<double>? ValuesSmallChainring { get; init; }
    /// <summary>
    /// Gets the UTC timestamp when this calculation was performed.
    /// </summary>
    public required DateTime CalculatedAt { get; init; }
    public required GearCalculInput UsedInputs { get; init; }

    public GearCalculResult()
    {

    }
}
