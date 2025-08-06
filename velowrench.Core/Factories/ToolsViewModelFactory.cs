
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Core.Enums;
using velowrench.Core.Interfaces;
using velowrench.Core.ViewModels.Tools;

namespace velowrench.Core.Factories;

public class ToolsViewModelFactory : IToolsViewModelFactory
{
    private readonly ILocalizer _localizer;

    public ToolsViewModelFactory(ILocalizer localizer)
    {
        _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
    }

    public IRoutableViewModel CreateRoutableViewModel(EVeloWrenchTools type, IScreen hostScreen)
    {
        ArgumentNullException.ThrowIfNull(hostScreen, nameof(hostScreen));

        switch (type)
        {
            case EVeloWrenchTools.ChainLengthCalculator:
                return new ChainLengthCalculatorViewModel(hostScreen, _localizer);
            case EVeloWrenchTools.ChainlineCalculator:
                return new ChainlineCalculatorViewModel(hostScreen, _localizer);
            case EVeloWrenchTools.DrivetrainRatioCalculator:
                return new DrivetrainRatioCalculatorViewModel(hostScreen, _localizer);
                case EVeloWrenchTools.RolloutCalculator:
                    return new RolloutCalculatorViewModel(hostScreen, _localizer);
            default:
                throw new NotSupportedException(type.ToString());
        }
    }
}