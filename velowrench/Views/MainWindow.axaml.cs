using Avalonia.Controls;
using Avalonia.ReactiveUI;
using ReactiveUI;
using velowrench.ViewModels;

namespace velowrench.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        this.WhenActivated(disposables => { });
        InitializeComponent();
    }
}