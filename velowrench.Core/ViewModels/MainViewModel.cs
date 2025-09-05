using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using velowrench.Core.Interfaces;
using velowrench.Core.ViewModels.Home;

namespace velowrench.Core.ViewModels;

/// <summary>
/// Main view model that orchestrates the application's primary user interface and navigation.
/// </summary>
public sealed partial class MainViewModel : ObservableObject, IHostViewModel, IDisposable
{
    private INavigationService _navigationService;

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

    public ToolbarViewModel Toolbar { get; }

    public MainViewModel(ToolbarViewModel toolbar)
    {
        this.Toolbar = toolbar ?? throw new ArgumentNullException(nameof(toolbar));
    }

    /// <summary>
    /// Initializes the main view model with the navigation service and navigates to the home page.
    /// This should be called after the dependency injection container is fully configured.
    /// </summary>
    public void Initialize(INavigationService navigationService)
    {
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        _navigationService.CurrentViewModelChanging += OnCurrentViewModelChanging;
        _navigationService.CurrentViewModelChanged += OnCurrentViewModelChanged;
        _navigationService.NavigateToHomeAsync();
    }

    private void OnCurrentViewModelChanging(object? sender, EventArgs e)
    {
        this.Toolbar.Reset();
    }

    /// <summary>
    /// Handles navigation events and updates the current content and navigation states.
    /// </summary>
    private void OnCurrentViewModelChanged(object? sender, EventArgs e)
    {
        Dispatcher.UIThread.Invoke(() =>
        {
            this.CurrentContent = _navigationService.CurrentViewModel;
            this.CanNavigateBack = _navigationService?.CanNavigateBack ?? false;
        });
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

    public void Dispose()
    {
        if (_navigationService != null)
        {
            _navigationService.CurrentViewModelChanged -= OnCurrentViewModelChanged;
            _navigationService.Dispose();
        }
    }
}
