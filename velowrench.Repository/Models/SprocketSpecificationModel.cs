using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace velowrench.Repository.Models;

public record SprocketSpecificationModel
{
    public string Label { get; init; }
    public int TeethCount { get; init; }

    public SprocketSpecificationModel(string label, int teethCount)
    {
        Label = label;
        TeethCount = teethCount;
    }
}
