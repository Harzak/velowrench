using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Core.Interfaces;
using velowrench.Core.Services;
using velowrench.Core.ViewModels.Base;

namespace velowrench.Core.ViewModels.Tools;

public partial class DrivetrainRatioCalculatorViewModel : BaseRoutableViewModel
{
    public override string Name { get; }

    public DrivetrainRatioCalculatorViewModel(INavigationService navigationService, ILocalizer localizer) : base(navigationService)
    {
        Name = localizer.GetString("DrivetrainRatioCalculator");
    }
}