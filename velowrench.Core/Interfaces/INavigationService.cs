using velowrench.Core.Enums;
using velowrench.Core.Navigation.Context;

namespace velowrench.Core.Interfaces;

/// <summary>
/// Service interface for managing navigation between different views and view models in the application.
/// </summary>
public interface INavigationService : IDisposable
{

    /// <summary>
    /// Gets a value indicating whether backward navigation is possible.
    /// </summary>
    bool CanNavigateBack { get; }

    /// <summary>
    /// Gets the currently active view model.
    /// </summary>
    /// <value>
    /// The current routable view model, or null if no view model is active.
    /// </value>
    IRoutableViewModel? CurrentViewModel { get; }

    event EventHandler<EventArgs>? CurrentViewModelChanging;

    event EventHandler<EventArgs>? CurrentViewModelChanged;

    /// <summary>
    /// Navigates to the home page and clears the navigation stack.
    /// </summary>
    Task NavigateToHomeAsync(NavigationParameters? parameters = null);

    Task NavigateToProfileAsync(NavigationParameters? parameters = null);

    /// <summary>
    /// Navigates to a specific tool page.
    /// </summary>
    Task NavigateToToolAsync(EVeloWrenchTools toolType, NavigationParameters? parameters = null);

    /// <summary>
    /// Navigates to the help page for a specific tool.
    /// </summary>
    Task NavigateToHelpAsync(EVeloWrenchTools toolType, NavigationParameters? parameters = null);

    /// <summary>
    /// Navigates to the specified view model.
    /// </summary>
    Task NavigateToAsync(IRoutableViewModel viewModel, NavigationParameters? parameters = null);

    /// <summary>
    /// Navigates back to the previous view in the navigation stack.
    /// </summary>
    Task NavigateBackAsync(NavigationParameters? parameters = null);

}