namespace velowrench.Calculations.Calculs.Transmission.ChainLength;

public record ChainLengthCalculResult
{
    public required int ChainLinks { get; init; }
    public required double ChainLengthInch { get; init; }
    public required DateTime CalculatedAt { get; init; }
    public required ChainLengthCalculInput UsedInputs { get; init; }
}