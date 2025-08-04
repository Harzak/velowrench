using CommunityToolkit.Mvvm.ComponentModel;

namespace velowrench.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _greeting = "Welcome to Avalonia!";
}
