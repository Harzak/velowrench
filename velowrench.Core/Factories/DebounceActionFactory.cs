using Avalonia.Threading;
using velowrench.Core.Interfaces;
using velowrench.Core.Services;

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
