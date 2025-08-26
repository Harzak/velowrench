using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace velowrench.Calculations.Validation.Results;

/// <summary>
/// Represents a validation error with contextual information.
/// </summary>
public sealed record ValidationError
{
    public required string PropertyName { get; init; }
    public required string Message { get; init; }
}