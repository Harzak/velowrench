using Avalonia.Styling;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitsNet.Units;

namespace velowrench.Core.Interfaces;

/// <summary>
/// Represents the configuration settings for the app.
/// </summary>
public interface IAppConfiguration
{
    /// <summary>
    /// Gets the collection of cultures supported by the application.
    /// </summary>
    IReadOnlyCollection<CultureInfo> SupportedCultures { get; }
    /// <summary>
    /// Gets or sets the culture information used for formatting and parsing operations.
    /// </summary>
    CultureInfo CurrentCulture { get; set; }

    /// <summary>
    /// Gets the collection of available themes that can be applied to the application.
    /// </summary>
    IReadOnlyCollection<ThemeVariant> AvailableThemes { get; }
    /// <summary>
    /// Gets or sets the current theme variant applied to the application.
    /// </summary>
    ThemeVariant CurrentTheme { get; set; }

    /// <summary>
    /// Gets or sets the preferred unit of measurement for length unit.
    /// </summary>
    LengthUnit PreferredLengthUnit { get; set; }
    /// <summary>
    /// Gets or sets the preferred unit of measurement for distance unit.
    /// </summary>
    LengthUnit PreferredDistanceUnit { get; set; }
    /// <summary>
    /// Gets or sets the preferred unit of measurement for speed uinit.
    /// </summary>
    SpeedUnit PreferredSpeedUnit { get; set; }

    Version AppVersion { get; }

    Task InitializeAsync();
    Task SaveAsync();
}
