using velowrench.Core.Enums;
using velowrench.Core.Interfaces;
using velowrench.Core.Navigation.Context;

namespace velowrench.Core.Navigation;

/// <summary>
/// Implementation of the navigation service that manages view model navigation and maintains a navigation stack.
/// </summary>
public sealed class NavigationService : INavigationService
{
    private readonly INavigationHandler _handler;
    private readonly IViewModelFactory _viewModelFactory;

    /// <inheritdoc/>
    public IRoutableViewModel? CurrentViewModel => _handler.ActiveViewModel;

    /// <inheritdoc/>
    public bool CanNavigateBack => _handler.CanPop;

    /// <inheritdoc/>
    public event EventHandler<EventArgs>? CurrentViewModelChanging;

    /// <inheritdoc/>
    public event EventHandler<EventArgs>? CurrentViewModelChanged;

    public NavigationService(INavigationHandler handler, IViewModelFactory viewModelFactory)
    {
        _handler = handler ?? throw new ArgumentNullException(nameof(handler));
        _viewModelFactory = viewModelFactory ?? throw new ArgumentNullException(nameof(viewModelFactory));

        _handler.ActiveViewModelChanging += OnActiveViewModelChanging;
        _handler.ActiveViewModelChanged += OnActiveViewModelChanged;
    }

    /// <inheritdoc/>
    public async Task NavigateToHomeAsync(NavigationParameters? parameters = null)
    {
        IRoutableViewModel homeViewModel = _viewModelFactory.CreateHomeViewModel(this);
        await _handler.ClearAsync().ConfigureAwait(false);
        await NavigateToAsync(homeViewModel).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task NavigateToProfileAsync(NavigationParameters? parameters = null)
    {
        IRoutableViewModel profileViewModel = _viewModelFactory.CreateProfileViewModel(this);
        await NavigateToAsync(profileViewModel).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task NavigateToToolAsync(EVeloWrenchTools toolType, NavigationParameters? parameters = null)
    {
        IRoutableViewModel toolViewModel = _viewModelFactory.CreateToolViewModel(toolType, this);
        await NavigateToAsync(toolViewModel).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task NavigateToHelpAsync(EVeloWrenchTools toolType, NavigationParameters? parameters = null)
    {
        IRoutableViewModel toolViewModel = _viewModelFactory.CreateHelpViewModel(toolType, this);
        await NavigateToAsync(toolViewModel).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task NavigateToAsync(IRoutableViewModel viewModel, NavigationParameters? parameters = null)
    {
        ArgumentNullException.ThrowIfNull(viewModel);

        NavigationContext context = new(parameters);
        await _handler.PushAsync(viewModel, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task NavigateBackAsync(NavigationParameters? parameters = null)
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
        if (_handler != null)
        {
            _handler.ActiveViewModelChanging -= OnActiveViewModelChanging;
            _handler.ActiveViewModelChanged -= OnActiveViewModelChanged;
            _handler.Dispose();
        }
    }
}