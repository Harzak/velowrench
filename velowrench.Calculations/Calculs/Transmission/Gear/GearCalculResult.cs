namespace velowrench.Calculations.Calculs.Transmission.Gear;

/// <summary>
/// Represents the result of a gear ratio calculations.
/// </summary>
public sealed record GearCalculResult : BaseCalculResult<GearCalculInput>
{
    public required IList<double> ValuesLargeOrUniqueChainring { get; init; }
    public IList<double>? ValuesMediumChainring { get; init; }
    public IList<double>? ValuesSmallChainring { get; init; }

    public GearCalculResult()
    {

    }
}
