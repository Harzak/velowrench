namespace velowrench.Calculations.Calculs.Transmission.ChainLength;

public record ChainLengthCalculResult
{
    public required int ChainLengthInLinks { get; init; }
    public required double ChainLengthInInches { get; init; }
    public required double ChainLengthInMm { get; init; }
    public required DateTime CalculatedAt { get; init; }
    public required ChainLengthCalculInput UsedInput { get; init; }
}