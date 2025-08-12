using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Repository.Models;

namespace velowrench.Repository.Extensions;

public static class WheelSpecificationModelExtensions
{
    public static WheelSpecificationModel GetMostUsedWheel(this IEnumerable<WheelSpecificationModel> source)
    {
        WheelSpecificationModel? dearOne = source?.FirstOrDefault(x => x.Label.StartsWith("26''", StringComparison.CurrentCulture));

        return dearOne ?? throw new InvalidOperationException("No favorite wheel found in the collection :(");
    }
}
