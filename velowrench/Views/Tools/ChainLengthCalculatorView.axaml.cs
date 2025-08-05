using Avalonia.Controls;
using Avalonia.ReactiveUI;
using ReactiveUI;
using velowrench.ViewModels.Tools;

namespace velowrench.Views.Tools;

public partial class ChainLengthCalculatorView : ReactiveUserControl<ChainLengthCalculatorViewModel>
{
    public ChainLengthCalculatorView()
    {
        InitializeComponent();
        this.WhenActivated(disposables => { });
    }
}