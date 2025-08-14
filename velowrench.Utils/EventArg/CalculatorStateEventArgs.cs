using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
