using velowrench.Repository.Models;

namespace velowrench.Repository.Extensions;

/// <summary>
/// Provides extension methods for <see cref="WheelSpecificationModel"/> collections
/// to support common selection and filtering operations in user interface scenarios.
/// </summary>
public static class WheelSpecificationModelExtensions
{
    /// <summary>
    /// Retrieves the most commonly used wheel specification from a collection.
    /// </summary>
    public static WheelSpecificationModel GetMostUsedWheel(this IEnumerable<WheelSpecificationModel> source)
    {
        WheelSpecificationModel? dearOne = source?.FirstOrDefault(x => x.Label.StartsWith("26''", StringComparison.CurrentCulture));

        return dearOne ?? throw new InvalidOperationException("No favorite wheel found in the collection :(");
    }
}
