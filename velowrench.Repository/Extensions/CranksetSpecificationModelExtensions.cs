using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Repository.Models;

namespace velowrench.Repository.Extensions;

public static class CranksetSpecificationModelExtensions
{
    public static CranksetSpecificationModel GetMostUsedCrankset(this IEnumerable<CranksetSpecificationModel> source)
    {
        CranksetSpecificationModel? mostUsed = source?.FirstOrDefault(x => x.Length == 170);

        return mostUsed ?? throw new InvalidOperationException("No most used crank found in the collection.");
    }
}