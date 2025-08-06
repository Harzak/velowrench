using Avalonia.Controls;
using Avalonia.ReactiveUI;
using velowrench.Core.ViewModels.Home;

namespace velowrench.UI.Views.Home;

public partial class HomeView : ReactiveUserControl<HomeViewModel>
{
    public HomeView()
    {
        InitializeComponent();
    }
}