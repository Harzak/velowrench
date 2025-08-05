using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace velowrench.Interfaces;

public interface ILocalizer
{
    string GetString(string key);
    string GetString(string key, params object[] args);
}
