using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Core.Enums;
using velowrench.Core.Interfaces;
using velowrench.Core.ViewModels.Home;

namespace velowrench.Core.Services;

public class NavigationService : INavigationService
{
    private readonly Stack<IRoutableViewModel> _navigationStack = new();
    private readonly IViewModelFactory _viewModelFactory;

    public event EventHandler<ViewModelChangedEventArgs>? CurrentViewModelChanged;
    public IRoutableViewModel? CurrentViewModel { get; private set; }
    public bool CanNavigateBack => _navigationStack.Count > 1 && CurrentViewModel != null;

    public NavigationService(IViewModelFactory viewModelFactory)
    {
        _viewModelFactory = viewModelFactory ?? throw new ArgumentNullException(nameof(viewModelFactory));
    }

    public void NavigateToHome()
    {
        IRoutableViewModel homeViewModel = _viewModelFactory.CreateHomeViewModel(this);
        NavigateTo(homeViewModel);
    }

    public void NavigateToTool(EVeloWrenchTools toolType)
    {
        IRoutableViewModel toolViewModel = _viewModelFactory.CreateToolViewModel(toolType, this);
        NavigateTo(toolViewModel);
    }

    public void NavigateBack()
    {
        if (!CanNavigateBack)
        {
            return;
        }

        IRoutableViewModel previous = CurrentViewModel;
        _navigationStack.Pop(); 
        CurrentViewModel = _navigationStack.Peek();

        CurrentViewModelChanged?.Invoke(this, new ViewModelChangedEventArgs(previous, CurrentViewModel));
    }

    public void ClearNavigationStack()
    {
        _navigationStack.Clear();
        CurrentViewModel = null;
    }

    private void NavigateTo(IRoutableViewModel viewModel)
    {
        IRoutableViewModel? previous = CurrentViewModel;
        _navigationStack.Push(viewModel);
        CurrentViewModel = viewModel;

        CurrentViewModelChanged?.Invoke(this, new ViewModelChangedEventArgs(previous, CurrentViewModel));
    }
}

public interface INavigationService
{
    void NavigateToHome();
    void NavigateToTool(EVeloWrenchTools toolType);
    void NavigateBack();
    bool CanNavigateBack { get; }
    IRoutableViewModel? CurrentViewModel { get; }

    event EventHandler<ViewModelChangedEventArgs> CurrentViewModelChanged;
    void ClearNavigationStack();
}

public class ViewModelChangedEventArgs : EventArgs
{
    public object? PreviousViewModel { get; }
    public object CurrentViewModel { get; }

    public ViewModelChangedEventArgs(object? previous, object current)
    {
        PreviousViewModel = previous;
        CurrentViewModel = current;
    }
}