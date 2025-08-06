using Avalonia.Controls;
using Avalonia.ReactiveUI;
using ReactiveUI;
using velowrench.Core.ViewModels.Tools;

namespace velowrench.UI.Views.Tools;

public partial class ChainLengthCalculatorView : ReactiveUserControl<ChainLengthCalculatorViewModel>
{
    public ChainLengthCalculatorView()
    {
        InitializeComponent();
        this.WhenActivated(disposables => { });
    }
}