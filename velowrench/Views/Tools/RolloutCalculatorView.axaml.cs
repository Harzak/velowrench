using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using velowrench.ViewModels.Tools;

namespace velowrench.Views.Tools;

public partial class RolloutCalculatorView : ReactiveUserControl<RolloutCalculatorViewModel>
{
    public RolloutCalculatorView()
    {
        InitializeComponent();
        this.WhenActivated(disposables => { });
    }
}