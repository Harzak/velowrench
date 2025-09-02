namespace velowrench.Utils.Interfaces;

public interface IDebounceActionFactory
{
    IDebounceAction CreateDebounceUIAction(Action action, int delayMs = 300);
}
