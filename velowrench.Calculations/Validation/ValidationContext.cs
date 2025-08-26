using System.Globalization;
using UnitsNet.Units;
using velowrench.Calculations.Interfaces;
using velowrench.Utils.Enums;

namespace velowrench.Core.Validation;

/// <summary>
/// Provides contextual information for validation operations.
/// </summary>
public sealed record ValidationContext
{
    public EValidationMode ValidationMode { get; init; }
    public object? SessionContext { get; init; }
    public CultureInfo Culture { get; init; }   

    public ValidationContext()
    {
        this.ValidationMode = EValidationMode.Immediate;
        this.Culture = CultureInfo.InvariantCulture;
    }
}