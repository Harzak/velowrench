using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Interfaces;
using velowrench.ViewModels.Base;

namespace velowrench.ViewModels.Tools;

public class DrivetrainRatioCalculatorViewModel : BaseRoutableViewModel
{
    public override string Name { get; }

    public DrivetrainRatioCalculatorViewModel(IScreen hostScreen, ILocalizer localizer) : base(hostScreen)
    {
        Name = localizer.GetString("DrivetrainRatioCalculator");
    }
}