using Avalonia.Controls;
using Avalonia.ReactiveUI;
using ReactiveUI;
using velowrench.ViewModels;

namespace velowrench.Views;

public partial class ChainLengthCalculatorView : ReactiveUserControl<ChainLengthCalculatorViewModel>
{
    public ChainLengthCalculatorView()
    {
        InitializeComponent();
        this.WhenActivated(disposables => { });
    }
}