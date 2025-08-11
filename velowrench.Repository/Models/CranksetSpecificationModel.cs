using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace velowrench.Repository.Models;

public sealed record CranksetSpecificationModel
{
    public string Label { get; init; }

    /// <summary>
    /// Represents a crank arm length
    /// </summary>
    /// <remarks>
    /// Crank arm lengths are always expressed in milimeter in the industry.
    /// </remarks>
    public double Length { get; init; }

    public CranksetSpecificationModel(string label, double length)
    {
        this.Label = label;
        this.Length = length;
    }
}
