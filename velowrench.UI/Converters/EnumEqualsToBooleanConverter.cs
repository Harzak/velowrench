using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace velowrench.UI.Converters;

/// <summary>
/// Converter that checks if an enum value equals a specified target value.
/// Returns true if the values are equal, false otherwise.
/// </summary>
public class EnumEqualsToBooleanConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null || parameter == null)
        {
            return false;
        }

        if (value.GetType().IsEnum && parameter.GetType().IsEnum)
        {
            return value.Equals(parameter);
        }

        return false;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException("EnumEqualsToBooleanConverter does not support two-way binding.");
    }
}