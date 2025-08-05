using Avalonia.Controls;
using Avalonia.ReactiveUI;
using velowrench.ViewModels;

namespace velowrench.Views;

public partial class HomeView : ReactiveUserControl<HomeViewModel>
{
    public HomeView()
    {
        InitializeComponent();
    }
}