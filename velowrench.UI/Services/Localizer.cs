using velowrench.Core.Interfaces;
using velowrench.UI.Resources;

namespace velowrench.UI.Services;

public sealed class Localizer : ILocalizer
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