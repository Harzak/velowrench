using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Core.Interfaces;

namespace velowrench.Core.EventArg;

public class ViewModelRegistryItemEventArgs : EventArgs
{
    public Guid Identifier { get; }

    public ViewModelRegistryItemEventArgs(Guid identifier)
    {
        this.Identifier = identifier;
    }
}
