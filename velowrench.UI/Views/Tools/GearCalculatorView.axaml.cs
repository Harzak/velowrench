using Avalonia.Controls;
using System;
using velowrench.Core.ViewModels.Tools;

namespace velowrench.UI.Views.Tools;

public partial class GearCalculatorView : UserControl
{
    public GearCalculatorView()
    {
        InitializeComponent();
    }

    private void OnSprocketDropDownClosed(object sender, EventArgs e)
    {
        if (this.DataContext is GearCalculatorViewModel vm)
        {
            vm.OnSprocketSelectionChanged();
        }
    }
}