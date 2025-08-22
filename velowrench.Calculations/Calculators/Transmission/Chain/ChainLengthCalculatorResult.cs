using UnitsNet.Units;
using velowrench.Calculations.Units;

namespace velowrench.Calculations.Calculators.Transmission.Chain;

/// <summary>
/// Represents the result of a chain length calculation for bicycle transmissions.
/// </summary>
public sealed record ChainLengthCalculatorResult : BaseCalculatorResult<ChainLengthCalculatorInput>
{
    /// <summary>
    /// Gets the number of chain links required for the calculated chain length.
    /// </summary>
    public required int ChainLinks { get; init; }
    
    /// <summary>
    /// Gets the calculated chain length in inches.
    /// </summary>
    public required double ChainLengthIn { get; init; }
}