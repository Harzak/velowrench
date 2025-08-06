using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace velowrench.Core.Interfaces;

public interface IRoutableViewModel
{
    public string Name { get; }
    public string UrlPathSegment { get; }
}
