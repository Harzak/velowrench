using velowrench.Core.Enums;
using velowrench.Core.EventArg;
using velowrench.Core.Interfaces;

namespace velowrench.Core.Services;

/// <summary>
/// Implementation of the navigation service that manages view model navigation and maintains a navigation stack.
/// </summary>
public sealed class NavigationService : INavigationService
{
    private readonly Stack<IRoutableViewModel> _navigationStack = new();
    private readonly IViewModelFactory _viewModelFactory;

    /// <summary>
    /// Event raised when the current view model changes during navigation.
    /// </summary>
    public event EventHandler<ViewModelChangedEventArgs>? CurrentViewModelChanged;

    /// <summary>
    /// Gets the currently active view model.
    /// </summary>
    public IRoutableViewModel? CurrentViewModel { get; private set; }

    /// <summary>
    /// Gets a value indicating whether backward navigation is possible.
    /// </summary>
    public bool CanNavigateBack => _navigationStack.Count > 1 && CurrentViewModel != null;

    public NavigationService(IViewModelFactory viewModelFactory)
    {
        _viewModelFactory = viewModelFactory ?? throw new ArgumentNullException(nameof(viewModelFactory));
    }

    /// <summary>
    /// Navigates to the home page and clears the navigation stack.
    /// </summary>
    public void NavigateToHome()
    {
        IRoutableViewModel homeViewModel = _viewModelFactory.CreateHomeViewModel(this);
        _navigationStack.Clear();
        NavigateTo(homeViewModel);
    }

    /// <summary>
    /// Navigates to a specific tool page.
    /// </summary>
    public void NavigateToTool(EVeloWrenchTools toolType)
    {
        IRoutableViewModel toolViewModel = _viewModelFactory.CreateToolViewModel(toolType, this);
        NavigateTo(toolViewModel);
    }

    /// <summary>
    /// Navigates to the help page for a specific tool.
    /// </summary>
    public void NavigateToHelp(EVeloWrenchTools toolType)
    {
        IRoutableViewModel toolViewModel = _viewModelFactory.CreateHelpViewModel(toolType, this);
        NavigateTo(toolViewModel);
    }

    /// <summary>
    /// Navigates back to the previous view in the navigation stack.
    /// </summary>
    public void NavigateBack()
    {
        if (!CanNavigateBack)
        {
            return;
        }

        IRoutableViewModel previous = CurrentViewModel!;
        _navigationStack.Pop();
        CurrentViewModel = _navigationStack.Peek();

        CurrentViewModelChanged?.Invoke(this, new ViewModelChangedEventArgs(previous, CurrentViewModel));
    }

    /// <summary>
    /// Clears the entire navigation stack and sets the current view model to null.
    /// </summary>
    public void ClearNavigationStack()
    {
        _navigationStack.Clear();
        CurrentViewModel = null;
    }

    /// <summary>
    /// Navigates to the specified view model by adding it to the navigation stack.
    /// </summary>
    private void NavigateTo(IRoutableViewModel viewModel)
    {
        IRoutableViewModel? previous = CurrentViewModel;
        _navigationStack.Push(viewModel);
        CurrentViewModel = viewModel;

        CurrentViewModelChanged?.Invoke(this, new ViewModelChangedEventArgs(previous, CurrentViewModel));
    }
}