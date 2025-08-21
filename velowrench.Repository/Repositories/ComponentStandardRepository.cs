using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Repository.Catalogs;
using velowrench.Repository.Interfaces;
using velowrench.Repository.Models;

namespace velowrench.Repository.Repositories;

/// <summary>
/// Provides access to standard bicycle component specifications through a hard-coded data catalog.
/// Implements <see cref="IComponentStandardRepository"/> using static data collections for access to standard bicycle component information.
/// </summary>
public sealed class ComponentStandardRepository : IComponentStandardRepository
{
    /// <summary>
    /// Retrieves all available crankset specifications from the hard-coded catalog.
    /// </summary>
    public IReadOnlyCollection<CranksetSpecificationModel> GetAllCranksetSpecifications()
    {
        return HardCodedComponentStandardCatalog.GetAllCranksetSpecifications;
    }

    /// <summary>
    /// Retrieves all available cadence specifications from the hard-coded catalog.
    /// </summary>
    public IReadOnlyCollection<CadenceModel> GetAllCandences()
    {
        return HardCodedComponentStandardCatalog.GetMostUsedCandences;
    }

    /// <summary>
    /// Retrieves the most commonly used wheel specifications from the hard-coded catalog.
    /// </summary>
    public IReadOnlyCollection<WheelSpecificationModel> GetMostCommonWheelSpecifications()
    {
        return HardCodedComponentStandardCatalog.GetMostCommonWheelSpecifications;
    }

    /// <summary>
    /// Retrieves the most common sprocket specifications from the hard-coded catalog.
    /// </summary>
    public IReadOnlyCollection<SprocketSpecificationModel> GetMostCommonSprocketSpecifications()
    {
        return HardCodedComponentStandardCatalog.GetMostCommonSprocketSpecification;
    }

    public IReadOnlyCollection<int> GetMostCommonWheelSpokeCount()
    {
        return HardCodedComponentStandardCatalog.GetMostCommonWheelSpokeCount;
    }

    public IReadOnlyCollection<int> GetMostCommonSpokeLacingPattern()
    {
        return HardCodedComponentStandardCatalog.GetMostCommonSpokeLacingPattern;
    }
}