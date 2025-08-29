using Avalonia.Threading;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using velowrench.Core.Enums;
using velowrench.Core.EventArg;
using velowrench.Core.Interfaces;
using velowrench.Core.LogMessages;

namespace velowrench.Core.Services;

/// <summary>
/// Implementation of the navigation service that manages view model navigation and maintains a navigation stack.
/// </summary>
public sealed class NavigationService : INavigationService
{
    private readonly ILogger<NavigationService> _logger;
    private readonly IHostViewModel _host;
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
    public bool CanNavigateBack => _navigationStack.Count > 1 && this.CurrentViewModel != null;

    public NavigationService(IHostViewModel host, IViewModelFactory viewModelFactory, ILogger<NavigationService> logger)
    {
        _host = host ?? throw new ArgumentNullException(nameof(host));
        _viewModelFactory = viewModelFactory ?? throw new ArgumentNullException(nameof(viewModelFactory));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Navigates to the home page and clears the navigation stack.
    /// </summary>
    public async Task NavigateToHomeAsync()
    {
        IRoutableViewModel homeViewModel = _viewModelFactory.CreateHomeViewModel(this);
        _navigationStack.Clear();
        await this.NavigateToAsync(homeViewModel).ConfigureAwait(false);
    }

    /// <summary>
    /// Navigates to a specific tool page.
    /// </summary>
    public async Task NavigateToToolAsync(EVeloWrenchTools toolType)
    {
        IRoutableViewModel toolViewModel = _viewModelFactory.CreateToolViewModel(toolType, this);
        await this.NavigateToAsync(toolViewModel).ConfigureAwait(false);
    }

    /// <summary>
    /// Navigates to the help page for a specific tool.
    /// </summary>
    public async Task NavigateToHelpAsync(EVeloWrenchTools toolType)
    {
        IRoutableViewModel toolViewModel = _viewModelFactory.CreateHelpViewModel(toolType, this);
        await this.NavigateToAsync(toolViewModel).ConfigureAwait(false);
    }

    /// <summary>
    /// Navigates to the specified view model by adding it to the navigation stack.
    /// </summary>
    public Task NavigateToAsync(IRoutableViewModel viewModel)
    {
        _navigationStack.Push(viewModel);
        return this.TrySetCurrentViewModelAsync(viewModel);
    }

    /// <summary>
    /// Navigates back to the previous view in the navigation stack.
    /// </summary>
    public Task NavigateBackAsync()
    {
        if (!this.CanNavigateBack)
        {
            return Task.CompletedTask;
        }

        _navigationStack.Pop();
        return this.TrySetCurrentViewModelAsync(_navigationStack.Peek());
    }

    private async Task<bool> TrySetCurrentViewModelAsync(IRoutableViewModel viewModel)
    {
        if (viewModel == null)
        {
            return false;
        }
        const int UI_BUSY_DELAY = 700;
        
        IRoutableViewModel? previousViewModel = this.CurrentViewModel;
        try
        {
            Dispatcher.UIThread.Invoke(() => _host.IsBusy = true);
            Stopwatch sw = Stopwatch.StartNew();
            
            if (previousViewModel != null)
            {
                await previousViewModel.OnDestroyAsync().ConfigureAwait(false);
            }
            
            await viewModel.OnInitializedAsync().ConfigureAwait(false);

            Dispatcher.UIThread.Invoke(() =>
            {
                this.CurrentViewModel = viewModel;
                this.CurrentViewModelChanged?.Invoke(this, new ViewModelChangedEventArgs(previousViewModel, this.CurrentViewModel!));
            });
                   
            previousViewModel?.Dispose();

            sw.Stop();       
            if (sw.ElapsedMilliseconds < UI_BUSY_DELAY)
            {
                await Task.Delay(UI_BUSY_DELAY - (int)sw.ElapsedMilliseconds).ConfigureAwait(false);
            }
            return true;
        }
        catch (Exception ex)
        {
            NavigationServiceLogs.ViewModelInitializationFailed(_logger, viewModel.Name, ex);
            return false;
        }
        finally
        {
            Dispatcher.UIThread.Invoke(() => _host.IsBusy = false);
        }
    }

    /// <summary>
    /// Clears the entire navigation stack and sets the current view model to null.
    /// </summary>
    public void ClearNavigationStack()
    {
        _navigationStack.Clear();
        this.CurrentViewModel = null;
    }
}