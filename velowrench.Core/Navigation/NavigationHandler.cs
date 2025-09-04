using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Core.EventArg;
using velowrench.Core.Interfaces;
using velowrench.Core.LogMessages;
using velowrench.Core.Navigation.Context;
using velowrench.Core.Navigation.Guards;

namespace velowrench.Core.Navigation;

internal sealed class NavigationHandler : INavigationHandler
{
    private readonly ILogger _logger;
    private readonly IEnumerable<INavigationGuard> _navigationGuards;
    private readonly IHostViewModel _host;
    private readonly Timer _cleanupTimer;
    private readonly NavigationStack _stack;

    public event EventHandler<EventArgs>? ActiveViewModelChanged;

    public IRoutableViewModel? ActiveViewModel => _stack.Items.FirstOrDefault();
    public bool CanPop => _stack.Items.Count > 1;

    public NavigationHandler(IHostViewModel host, ILogger<NavigationHandler> logger)
    {
        _host = host ?? throw new ArgumentNullException(nameof(host));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _stack = new();
        _navigationGuards = [new CalculationInProgressGuard()];
        _cleanupTimer = new Timer(callback: OnCleanupTimerElapsed, 
                                  state: null, 
                                  dueTime: (int)TimeSpan.FromMinutes(2).TotalMilliseconds, 
                                  period: (int)TimeSpan.FromMinutes(2).TotalMilliseconds);
    }

    public async Task ClearAsync()
    {
        _stack.Clear();
        await GracefullyCleanupAllAsync().ConfigureAwait(false);
    }

    public async Task PushAsync(IRoutableViewModel viewModel, NavigationContext context)
    {
        foreach (INavigationGuard guard in _navigationGuards)
        {
            NavigationGuardResult result = await guard.CanNavigateAsync(ActiveViewModel, viewModel, context).ConfigureAwait(false);
            if (!result.CanNavigate)
            {
                return;
            }
        }

        _host.IsBusy = true;

        _stack.Push(viewModel);
        await viewModel.OnInitializedAsync().ConfigureAwait(false);

        _host.IsBusy = false;
        ActiveViewModelChanged?.Invoke(this, EventArgs.Empty);
    }

    public async Task PopAsync(NavigationContext context)
    {
        IRoutableViewModel? viewModel = _stack.Pop();
        if (viewModel != null)
        {
            await GracefullyCleanupAsync(viewModel).ConfigureAwait(false);
            ActiveViewModelChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private async void OnCleanupTimerElapsed(object? state)
    {
        await ForceCleanupAllAsync().ConfigureAwait(false);
    }

    private async Task GracefullyCleanupAllAsync()
    {
        foreach (IRoutableViewModel viewModel in _stack.Untracked)
        {
            await GracefullyCleanupAsync(viewModel).ConfigureAwait(false);
        }
    }

    private async Task GracefullyCleanupAsync(IRoutableViewModel viewModel)
    {
        try
        {
            await viewModel.OnDestroyAsync().ConfigureAwait(false);
            viewModel.Dispose();
            _stack.ClearUntrack();
            NavigationHandlerLogs.Destroyed(_logger, viewModel.Name);
        }
        catch (Exception ex)
        {
            NavigationHandlerLogs.DestroyFailed(_logger, viewModel.Name, ex);
        }
    }

    private async Task ForceCleanupAllAsync()
    {
        foreach (IRoutableViewModel viewModel in _stack.Untracked)
        {
            await ForceCleanupAsync(viewModel).ConfigureAwait(false);
        }
    }

    private async Task ForceCleanupAsync(IRoutableViewModel viewModel)
    {
        try
        {
            await viewModel.OnForceDestroyAsync().ConfigureAwait(false);
            viewModel.Dispose();
            _stack.ClearUntrack();
            NavigationHandlerLogs.ForceDestroyed(_logger, viewModel.Name);
        }
        catch (Exception ex)
        {
            NavigationHandlerLogs.ForceDestroyFailed(_logger, viewModel.Name, ex);
        }
    }

    public void Dispose()
    {
        _cleanupTimer?.Dispose();
        _stack?.Dispose();
    }
}
