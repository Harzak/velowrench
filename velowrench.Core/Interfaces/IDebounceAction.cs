namespace velowrench.Core.Interfaces;

public interface IDebounceAction : IDisposable
{
    void Execute();
}