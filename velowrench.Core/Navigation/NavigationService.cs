using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using velowrench.Core.Enums;
using velowrench.Core.EventArg;
using velowrench.Core.Interfaces;
using velowrench.Core.LogMessages;
using velowrench.Core.Navigation.Context;

namespace velowrench.Core.Navigation;

/// <summary>
/// Implementation of the navigation service that manages view model navigation and maintains a navigation stack.
/// </summary>
public sealed class NavigationService : INavigationService
{
    private readonly INavigationHandler _handler;
    private readonly IViewModelFactory _viewModelFactory;

    /// <summary>
    /// Gets the currently active view model.
    /// </summary>
    public IRoutableViewModel? CurrentViewModel => _handler.ActiveViewModel;

    /// <summary>
    /// Gets a value indicating whether backward navigation is possible.
    /// </summary>
    public bool CanNavigateBack => _handler.CanPop;


    public event EventHandler<EventArgs>? CurrentViewModelChanging;
    public event EventHandler<EventArgs>? CurrentViewModelChanged;

    public NavigationService(INavigationHandler handler, IViewModelFactory viewModelFactory)
    {
        _handler = handler ?? throw new ArgumentNullException(nameof(handler));
        _viewModelFactory = viewModelFactory ?? throw new ArgumentNullException(nameof(viewModelFactory));

        _handler.ActiveViewModelChanging += OnActiveViewModelChanging;
        _handler.ActiveViewModelChanged += OnActiveViewModelChanged;
    }

    /// <summary>
    /// Navigates to the home page and clears the navigation stack.
    /// </summary>
    public async Task NavigateToHomeAsync(NavigationParameters? parameters = null)
    {
        IRoutableViewModel homeViewModel = _viewModelFactory.CreateHomeViewModel(this);
        await _handler.ClearAsync().ConfigureAwait(false);
        await NavigateToAsync(homeViewModel).ConfigureAwait(false);
    }

    public async Task NavigateToProfileAsync(NavigationParameters? parameters = null)
    {
        IRoutableViewModel profileViewModel = _viewModelFactory.CreateProfileViewModel(this);
        await NavigateToAsync(profileViewModel).ConfigureAwait(false);
    }

    /// <summary>
    /// Navigates to a specific tool page.
    /// </summary>
    public async Task NavigateToToolAsync(EVeloWrenchTools toolType, NavigationParameters? parameters = null)
    {
        IRoutableViewModel toolViewModel = _viewModelFactory.CreateToolViewModel(toolType, this);
        await NavigateToAsync(toolViewModel).ConfigureAwait(false);
    }

    /// <summary>
    /// Navigates to the help page for a specific tool.
    /// </summary>
    public async Task NavigateToHelpAsync(EVeloWrenchTools toolType, NavigationParameters? parameters = null)
    {
        IRoutableViewModel toolViewModel = _viewModelFactory.CreateHelpViewModel(toolType, this);
        await NavigateToAsync(toolViewModel).ConfigureAwait(false);
    }

    /// <summary>
    /// Navigates to the specified view model by adding it to the navigation stack.
    /// </summary>
    public async Task NavigateToAsync(IRoutableViewModel viewModel, NavigationParameters? parameters = null)
    {
        ArgumentNullException.ThrowIfNull(viewModel);

        NavigationContext context = new(parameters);
        await _handler.PushAsync(viewModel, context).ConfigureAwait(false);
    }

    /// <summary>
    /// Navigates back to the previous view in the navigation stack.
    /// </summary>
    public  async Task NavigateBackAsync(NavigationParameters? parameters = null)
    {
        if (!CanNavigateBack)
        {
            return;
        }

        NavigationContext context = new(parameters);
        await _handler.PopAsync(context).ConfigureAwait(false);
    }

    private void OnActiveViewModelChanging(object? sender, EventArgs e)
    {
        this.CurrentViewModelChanging?.Invoke(this, e);
    }

    private void OnActiveViewModelChanged(object? sender, EventArgs e)
    {
        this.CurrentViewModelChanged?.Invoke(this, e);
    }

    public void Dispose()
    {
        if(_handler != null)
        {
            _handler.ActiveViewModelChanging -= OnActiveViewModelChanging;
            _handler.ActiveViewModelChanged -= OnActiveViewModelChanged;
            _handler.Dispose();
        }
    }
}