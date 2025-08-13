using velowrench.Repository.Models;

namespace velowrench.Repository.Extensions;

/// <summary>
/// Provides extension methods for <see cref="CranksetSpecificationModel"/> collections
/// to support default selection and common filtering operations.
/// </summary>
public static class CranksetSpecificationModelExtensions
{
    /// <summary>
    /// Retrieves the most commonly used crankset specification from a collection.
    /// </summary>
    public static CranksetSpecificationModel GetMostUsedCrankset(this IEnumerable<CranksetSpecificationModel> source)
    {
        CranksetSpecificationModel? mostUsed = source?.FirstOrDefault(x => x.Length == 170);

        return mostUsed ?? throw new InvalidOperationException("No most used crank found in the collection.");
    }
}