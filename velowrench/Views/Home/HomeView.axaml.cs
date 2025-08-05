using Avalonia.Controls;
using Avalonia.ReactiveUI;
using velowrench.ViewModels.Home;

namespace velowrench.Views.Home;

public partial class HomeView : ReactiveUserControl<HomeViewModel>
{
    public HomeView()
    {
        InitializeComponent();
    }
}