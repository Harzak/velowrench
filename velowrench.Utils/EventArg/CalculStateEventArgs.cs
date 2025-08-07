using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Utils.Enums;

namespace velowrench.Utils.EventArg;

public class CalculStateEventArgs : EventArgs
{
    public ECalculState State { get; set; }

    public CalculStateEventArgs(ECalculState state)
    {
        State = state;
    }
}
