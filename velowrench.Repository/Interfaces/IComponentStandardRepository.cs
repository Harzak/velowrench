using velowrench.Repository.Models;

namespace velowrench.Repository.Interfaces;

/// <summary>
/// Defines a contract for accessing standard bicycle component specifications and data.
/// Provides methods to retrieve collections of common bicycle components used in calculations
/// and user interface population throughout the application.
/// </summary>
public interface IComponentStandardRepository
{
    /// <summary>
    /// Retrieves all available crankset specifications.
    /// </summary>
    IReadOnlyCollection<CranksetSpecificationModel> GetAllCranksetSpecifications();

    /// <summary>
    /// Retrieves all available cadence specifications.
    /// </summary>
    IReadOnlyCollection<CadenceModel> GetAllCandences();

    /// <summary>
    /// Retrieves the most commonly used wheel specifications.
    /// </summary>
    IReadOnlyCollection<WheelSpecificationModel> GetMostCommonWheelSpecifications();

    /// <summary>
    /// Retrieves the most common sprocket specifications.
    /// </summary>
    IReadOnlyCollection<SprocketSpecificationModel> GetMostCommonSprocketSpecifications();
}

