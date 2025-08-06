using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Core.Interfaces;
using velowrench.Core.ViewModels.Base;

namespace velowrench.Core.ViewModels.Tools;

public class RolloutCalculatorViewModel : BaseRoutableViewModel
{
    public override string Name { get; }

    public RolloutCalculatorViewModel(IScreen hostScreen, ILocalizer localizer) : base(hostScreen)
    {
        Name = localizer.GetString("RolloutCalculator");
    }
}