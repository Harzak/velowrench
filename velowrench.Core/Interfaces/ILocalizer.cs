namespace velowrench.Core.Interfaces;

/// <summary>
/// Service interface for retrieving localized strings and text resources.
/// </summary>
public interface ILocalizer
{
    /// <summary>
    /// Gets the localized string for the specified resource key.
    /// </summary>
    /// <param name="key">The resource key to retrieve the localized string for.</param>
    /// <returns>The localized string corresponding to the specified key.</returns>
    string GetString(string key);

    /// <summary>
    /// Gets the localized string for the specified resource key and formats it with the provided arguments.
    /// </summary>
    /// <param name="key">The resource key to retrieve the localized string for.</param>
    /// <param name="args">The arguments to use for string formatting.</param>
    /// <returns>The formatted localized string corresponding to the specified key.</returns>
    string GetString(string key, params object[] args);
}
