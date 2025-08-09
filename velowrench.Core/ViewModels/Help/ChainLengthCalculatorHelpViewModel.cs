using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Core.Interfaces;
using velowrench.Core.ViewModels.Base;

namespace velowrench.Core.ViewModels.Help;

public class ChainLengthCalculatorHelpViewModel : BaseRoutableViewModel
{
    public override string Name => "Chain length Help";

    public ChainLengthCalculatorHelpViewModel(INavigationService navigationService, ILocalizer  localizer) : base(navigationService)
    {

    }
}
