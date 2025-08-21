using Avalonia.Data.Converters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace velowrench.UI.Converters;

/// <summary>
/// Converts a collection to a value treu or false depending on whether the collection has elements
/// </summary>
internal class NotEmptyCollectionToBooleanConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is IEnumerable enumerable)
        {
            return enumerable.Cast<object>().Any();
        }
        return false;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException("NotEmptyCollectionToBooleanConverter does not support two-way binding.");
    }
}

