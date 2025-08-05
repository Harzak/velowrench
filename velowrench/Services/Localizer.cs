using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Interfaces;
using velowrench.Resources;

namespace velowrench.Services;

public class Localizer : ILocalizer
{
    public string GetString(string key)
    {
        return Languages.ResourceManager.GetString(key) ?? key;
    }

    public string GetString(string key, params object[] args)
    {
        string format = GetString(key);
        return string.Format(format, args);
    }
}