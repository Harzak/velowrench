using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace velowrench.Repository.Models;

public sealed record CadenceModel
{
    public string Label { get; init; }
    public int Rpm { get; init; }

    public CadenceModel(string label, int rpm)
    {
        Label = label;
        Rpm = rpm;
    }
}
