
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Text;
using System.Threading.Tasks;
using velowrench.Core.Enums;
using velowrench.Core.Interfaces;
using velowrench.Core.Services;
using velowrench.Core.ViewModels.Home;

namespace velowrench.Core.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private IRoutableViewModel? _currentContent;

    public MainWindowViewModel(INavigationService  navigationService)
    {
        _navigationService = navigationService;

        _navigationService.CurrentViewModelChanged += (_, e) => CurrentContent = e.CurrentViewModel;
        _navigationService.NavigateToHome();
    }


    [RelayCommand]
    private void NavigateBack()
    {
        _navigationService.NavigateBack();
    }
}
