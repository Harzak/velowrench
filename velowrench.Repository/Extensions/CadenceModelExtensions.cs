using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Repository.Models;

namespace velowrench.Repository.Extensions;

public static class CadenceModelExtensions
{
    public static CadenceModel GetMostUsedCadence(this IEnumerable<CadenceModel> source)
    {
        CadenceModel? mostUsed = source?.FirstOrDefault(x => x.Rpm == 80);
        return mostUsed ?? throw new InvalidOperationException("No most used cadence found in the collection.");
    }
}