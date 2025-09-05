using velowrench.Core.Interfaces;
using velowrench.UI.Resources;

namespace velowrench.UI.Services;

/// <inheritdoc/>
public sealed class Localizer : ILocalizer
{
    /// <inheritdoc/>
    public string GetString(string key)
    {
        return Languages.ResourceManager.GetString(key) ?? key;
    }

    /// <inheritdoc/>
    public string GetString(string key, params object[] args)
    {
        string format = GetString(key);
        return string.Format(format, args);
    }
}