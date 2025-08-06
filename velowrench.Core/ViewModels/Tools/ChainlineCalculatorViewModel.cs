using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Core.Interfaces;
using velowrench.Core.ViewModels.Base;

namespace velowrench.Core.ViewModels.Tools;

public class ChainlineCalculatorViewModel : BaseRoutableViewModel
{
    public override string Name { get; }

    public ChainlineCalculatorViewModel(IScreen hostScreen, ILocalizer localizer) : base(hostScreen)
    {
        Name = localizer.GetString("ChainlineCalculator");
    }
}