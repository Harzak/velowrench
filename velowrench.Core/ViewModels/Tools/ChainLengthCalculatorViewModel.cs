using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using velowrench.Core.Interfaces;
using velowrench.Core.ViewModels.Base;

namespace velowrench.Core.ViewModels.Tools;

public class ChainLengthCalculatorViewModel : BaseRoutableViewModel
{
    public override string Name { get; }

    public ChainLengthCalculatorViewModel(IScreen hostScreen, ILocalizer localizer) : base(hostScreen)
    {
        Name = localizer.GetString("ChainLengthCalculator");
    }
}