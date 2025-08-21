using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace velowrench.UI.Converters;

/// <summary>
/// Converts a value to true of false based on whether the value is non-null.
/// </summary>
internal class NotNullToBooleanConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value != null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException("NullToBooleanConverter does not support two-way binding.");
    }
}
