using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Repository.Models;

namespace velowrench.Repository.Extensions;

/// <summary>
/// Provides extension methods for <see cref="CadenceModel"/> collections
/// to support default selection and common filtering operations.
/// </summary>
public static class CadenceModelExtensions
{
    /// <summary>
    /// Retrieves the most commonly used cadence specification from a collection.
    /// </summary>
    public static CadenceModel GetMostUsedCadence(this IEnumerable<CadenceModel> source)
    {
        CadenceModel? mostUsed = source?.FirstOrDefault(x => x.Rpm == 80);
        return mostUsed ?? throw new InvalidOperationException("No most used cadence found in the collection.");
    }
}