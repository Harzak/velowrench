using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Diagnostics;
using velowrench.Core.Interfaces;
using velowrench.Core.Services;

namespace velowrench.Core.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private IRoutableViewModel? _currentContent;

    public MainViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

        _navigationService.CurrentViewModelChanged += OnCurrentViewModelChanged;
        
        Debug.WriteLine("MainViewModel created, navigating to home...");
        _navigationService.NavigateToHome();
    }

    private void OnCurrentViewModelChanged(object? sender, ViewModelChangedEventArgs e)
    {
        Debug.WriteLine($"Navigation changed: {e.CurrentViewModel?.GetType().Name}");
        CurrentContent = e.CurrentViewModel;
    }

    [RelayCommand]
    private void NavigateBack()
    {
        Debug.WriteLine("NavigateBack command executed");
        _navigationService.NavigateBack();
    }
}
