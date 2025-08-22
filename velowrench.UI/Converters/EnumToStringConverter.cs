using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace velowrench.UI.Converters;

/// <summary>
/// Provides a value converter that converts an enumeration value to its string representation and vice versa.
/// </summary>
internal class EnumToStringConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value?.ToString() ?? string.Empty;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Enum.Parse(targetType, value?.ToString() ?? string.Empty);
    }
}
