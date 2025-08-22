using Avalonia.Data.Converters;
using System;
using System.Globalization;
using velowrench.Core.Units;

namespace velowrench.UI.Converters;

/// <summary>
/// Converts unit enumeration values to their display-friendly string representations and vice versa.
/// </summary>
public class UnitDisplayConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Enum unit)
        {
            UnitAbbreviation<Enum> unitAbbreviation = new(unit);
            return unitAbbreviation.Abbreviation;
        }

        return value?.ToString();
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}