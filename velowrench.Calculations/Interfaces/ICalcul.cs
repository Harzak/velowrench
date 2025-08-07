using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Utils.Enums;
using velowrench.Utils.EventArg;
using velowrench.Utils.Results;

namespace velowrench.Calculations.Interfaces;

public interface ICalcul 
{
    public ECalculState State { get;  }

    event EventHandler<CalculStateEventArgs>? StateChanged;
}

