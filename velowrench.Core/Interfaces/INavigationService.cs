using velowrench.Core.Enums;
using velowrench.Core.EventArg;

namespace velowrench.Core.Interfaces;

/// <summary>
/// Service interface for managing navigation between different views and view models in the application.
/// </summary>
public interface INavigationService
{
    /// <summary>
    /// Navigates to the home page and clears the navigation stack.
    /// </summary>
    void NavigateToHome();

    /// <summary>
    /// Navigates to a specific tool page.
    /// </summary>
    void NavigateToTool(EVeloWrenchTools toolType);

    /// <summary>
    /// Navigates to the help page for a specific tool.
    /// </summary>
    void NavigateToHelp(EVeloWrenchTools toolType);

    /// <summary>
    /// Navigates back to the previous view in the navigation stack.
    /// </summary>
    void NavigateBack();

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

    /// <summary>
    /// Event raised when the current view model changes during navigation.
    /// </summary>
    event EventHandler<ViewModelChangedEventArgs> CurrentViewModelChanged;

    /// <summary>
    /// Clears the entire navigation stack and sets the current view model to null.
    /// </summary>
    void ClearNavigationStack();
}