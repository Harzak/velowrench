using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using velowrench.Core.EventArg;
using velowrench.Core.Interfaces;

namespace velowrench.Core.ViewModels;

/// <summary>
/// Main view model that orchestrates the application's primary user interface and navigation.
/// </summary>
public sealed partial class MainViewModel : ObservableObject, IHostViewModel
{
    private INavigationService? _navigationService;

    /// <summary>
    /// Gets or sets the currently displayed content view model.
    /// </summary>
    /// <value>
    /// The routable view model that represents the main content area, or null if no content is displayed.
    /// </value>
    [ObservableProperty]
    private IRoutableViewModel? _currentContent;

    [ObservableProperty]
    private bool _isBusy;

    /// <summary>
    /// Gets or sets a value indicating whether backward navigation is possible.
    /// </summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(NavigateBackCommand))]
    private bool _canNavigateBack;

    /// <summary>
    /// Gets or sets a value indicating whether the current view has an associated help page.
    /// </summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ShowHelpPageCommand))]
    private bool _canShowHelpPage;

    [ObservableProperty]
    private bool _canShowContextMenu;

    // Parameterless constructor
    public MainViewModel()
    {
    }

    /// <summary>
    /// Initializes the main view model with the navigation service and navigates to the home page.
    /// This should be called after the dependency injection container is fully configured.
    /// </summary>
    public void Initialize(INavigationService navigationService)
    {
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        _navigationService.CurrentViewModelChanged += OnCurrentViewModelChanged;
        _navigationService.NavigateToHomeAsync();
    }

    /// <summary>
    /// Handles navigation events and updates the current content and navigation states.
    /// </summary>
    private void OnCurrentViewModelChanged(object? sender, ViewModelChangedEventArgs e)
    {
        this.CurrentContent = e.CurrentViewModel;
        this.CanNavigateBack = _navigationService?.CanNavigateBack ?? false;
        this.CanShowHelpPage = _navigationService?.CurrentViewModel?.CanShowHelpPage ?? false;
        this.CanShowContextMenu = _navigationService?.CurrentViewModel?.CanShowContextMenu ?? false;
    }

    /// <summary>
    /// Command to navigate back to the previous view in the navigation stack.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanNavigateBack))]
    private void NavigateBack()
    {
        _navigationService?.NavigateBackAsync();
        CanNavigateBack = _navigationService?.CanNavigateBack ?? false;
    }

    /// <summary>
    /// Command to display the help page for the current view.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanShowHelpPage))]
    private void ShowHelpPage()
    {
        _navigationService?.CurrentViewModel?.ShowHelpPage();
    }

    /// <summary>
    /// Command to display the help page for the current view.
    /// </summary>
    [RelayCommand]
    private void ResetToDefault()
    {
        _navigationService?.CurrentViewModel?.ResetToDefault();
    }
}
