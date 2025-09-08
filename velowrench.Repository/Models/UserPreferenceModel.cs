using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace velowrench.Repository.Models;

public sealed record UserPreferenceModel
{
    public required string AppLanguage { get; init; }
    public required string Theme { get; init; }
    public required string LengthUnit { get; init; }
    public required string DistanceUnit { get; init; }
    public required string SpeedUnit { get; init; }
}
