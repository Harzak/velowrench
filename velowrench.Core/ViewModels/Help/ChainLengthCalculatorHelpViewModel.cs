using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Core.Interfaces;
using velowrench.Core.ViewModels.Base;

namespace velowrench.Core.ViewModels.Help;

public partial class ChainLengthCalculatorHelpViewModel : BaseRoutableViewModel
{
    public override string Name => "Chain length Help";

    [ObservableProperty]
    private string _simpleEquationLaTeX = @"L = 2 \cdot C + \frac{F}{4} + \frac{R}{4} + 1";

    [ObservableProperty]
    private string _rigorousEquationLaTeX = @"L = 0.25 \cdot \left(F + R\right) + 2 \cdot \sqrt{C^{2} + \left(0.0796 \cdot \left(F - R\right)\right)^{2}}";

    public ChainLengthCalculatorHelpViewModel(INavigationService navigationService, ILocalizer  localizer) : base(navigationService)
    {

    }
}
