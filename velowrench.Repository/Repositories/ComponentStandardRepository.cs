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
    /// <inheritdoc/>
    public IReadOnlyCollection<CranksetSpecificationModel> GetAllCranksetSpecifications()
    {
        return HardCodedComponentStandardCatalog.GetAllCranksetSpecifications;
    }

    /// <inheritdoc/>
    public IReadOnlyCollection<CadenceModel> GetAllCandences()
    {
        return HardCodedComponentStandardCatalog.GetMostUsedCandences;
    }

    /// <inheritdoc/>
    public IReadOnlyCollection<WheelSpecificationModel> GetMostCommonWheelSpecifications()
    {
        return HardCodedComponentStandardCatalog.GetMostCommonWheelSpecifications;
    }

    /// <inheritdoc/>
    public IReadOnlyCollection<SprocketSpecificationModel> GetMostCommonSprocketSpecifications()
    {
        return HardCodedComponentStandardCatalog.GetMostCommonSprocketSpecification;
    }

    /// <inheritdoc/>
    public IReadOnlyCollection<int> GetMostCommonWheelSpokeCount()
    {
        return HardCodedComponentStandardCatalog.GetMostCommonWheelSpokeCount;
    }

    /// <inheritdoc/>
    public IReadOnlyCollection<SpokeLacingPatternModel> GetMostCommonSpokeLacingPattern()
    {
        return HardCodedComponentStandardCatalog.GetMostCommonSpokeLacingPattern;
    }
}