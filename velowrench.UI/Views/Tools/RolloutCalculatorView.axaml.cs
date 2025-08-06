using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using velowrench.Core.ViewModels.Tools;

namespace velowrench.UI.Views.Tools;

public partial class RolloutCalculatorView : ReactiveUserControl<RolloutCalculatorViewModel>
{
    public RolloutCalculatorView()
    {
        InitializeComponent();
        this.WhenActivated(disposables => { });
    }
}