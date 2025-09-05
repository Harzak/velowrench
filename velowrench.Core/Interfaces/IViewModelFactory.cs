using velowrench.Core.Enums;

namespace velowrench.Core.Interfaces;

/// <summary>
/// Factory interface for creating view model instances for different parts of the application.
/// </summary>
public interface IViewModelFactory
{
    /// <summary>
    /// Creates a tool-specific view model for the specified tool type.
    /// </summary>
    /// <returns>A routable view model instance for the specified tool.</returns>
    /// <exception cref="NotSupportedException">Thrown when the specified tool type is not supported.</exception>
    IRoutableViewModel CreateToolViewModel(EVeloWrenchTools type, INavigationService navigationService);

    /// <summary>
    /// Creates a help view model for the specified tool type.
    /// </summary>
    /// <returns>A routable view model instance for the tool's help page.</returns>
    /// <exception cref="NotSupportedException">Thrown when the specified tool type does not have help documentation.</exception>
    IRoutableViewModel CreateHelpViewModel(EVeloWrenchTools type, INavigationService navigationService);

    /// <summary>
    /// Creates the home page view model.
    /// </summary>
    /// <returns>A routable view model instance for the home page.</returns>
    IRoutableViewModel CreateHomeViewModel(INavigationService navigationService);

    /// <summary>
    /// Creates a new instance of a profile view model.
    /// </summary>
    /// <returns>An instance of <see cref="IRoutableViewModel"/> representing the profile view model.</returns>
    IRoutableViewModel CreateProfileViewModel(INavigationService navigationService);
}