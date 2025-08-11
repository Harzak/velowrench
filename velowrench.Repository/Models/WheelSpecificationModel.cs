using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace velowrench.Repository.Models;

public record WheelSpecificationModel
{
    public string Label { get; init; }
    public int BSDmm { get; init; }
    public double BSDin { get; init; }

    public WheelSpecificationModel(string label, double bSDin,int bSDmm)
    {
        Label = label;
        BSDmm = bSDmm;
        BSDin = bSDin;
    }
}
