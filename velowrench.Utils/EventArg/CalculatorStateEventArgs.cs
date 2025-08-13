using velowrench.Utils.Enums;

namespace velowrench.Utils.EventArg;

public class CalculatorStateEventArgs : EventArgs
{
    public ECalculatorState State { get; set; }

    public CalculatorStateEventArgs(ECalculatorState state)
    {
        State = state;
    }
}
