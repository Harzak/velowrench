using Avalonia.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Core.Actions;
using velowrench.Core.Interfaces;

namespace velowrench.Core.Factories;

public class DebounceActionFactory : IDebounceActionFactory
{
    public IDebounceAction CreateDebounceUIAction(Action action, int delayMs = 300)
    {
        return new DebounceAction(
            action,
            delayMs,
            dispatcher: async asyncAction => await Dispatcher.UIThread.InvokeAsync(asyncAction)
        );
    }
}
