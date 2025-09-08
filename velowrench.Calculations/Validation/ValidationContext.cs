using System.Globalization;
using velowrench.Utils.Enums;

namespace velowrench.Calculations.Validation;

/// <summary>
/// Provides contextual information for validation operations.
/// </summary>
public sealed record ValidationContext
{
    public EValidationMode ValidationMode { get; init; }
    public object? SessionContext { get; init; }
    public CultureInfo Culture { get; init; }

    public ValidationContext(EValidationMode mode)
    {
        this.ValidationMode = mode;
        this.Culture = CultureInfo.InvariantCulture;
    }
}