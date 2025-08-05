using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using velowrench.ViewModels.Tools;

namespace velowrench.Views.Tools;

public partial class ChainlineCalculatorView : ReactiveUserControl<ChainlineCalculatorViewModel>
{
    public ChainlineCalculatorView()
    {
        InitializeComponent();
        this.WhenActivated(disposables => { });
    }
}