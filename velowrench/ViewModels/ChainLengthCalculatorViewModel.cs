using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using velowrench.Interfaces;

namespace velowrench.ViewModels;

public class ChainLengthCalculatorViewModel : BaseRoutableViewModel
{
    public override string Name { get; }

    public ChainLengthCalculatorViewModel(IScreen hostScreen, ILocalizer localizer) : base(hostScreen)
    {
        this.Name = localizer.GetString("ChainLengthCalculator");
    }
}