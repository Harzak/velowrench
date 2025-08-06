using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using velowrench.Core.EventArg;
using velowrench.Core.Interfaces;

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

        _navigationService.NavigateToHome();
    }

    private void OnCurrentViewModelChanged(object? sender, ViewModelChangedEventArgs e)
    {
        CurrentContent = e.CurrentViewModel;
    }

    [RelayCommand]
    private void NavigateBack()
    {
        _navigationService.NavigateBack();
    }
}
