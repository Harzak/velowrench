using Avalonia.Data.Converters;
using System;
using System.Collections;
using System.Globalization;

namespace velowrench.UI.Converters;

/// <summary>
/// Converter that checks if a collection contains a specific item.
/// Returns true if the item is found in the collection, false otherwise.
/// </summary>
public class ContainsConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        // Value should be the collection (e.g., SelectedSprockets)
        // Parameter should be the item to check for (e.g., current SprocketSpecificationModel)
        
        if (value is IEnumerable collection && parameter != null)
        {
            foreach (var item in collection)
            {
                if (Equals(item, parameter))
                {
                    return true;
                }
            }
        }
        
        return false;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException("ContainsConverter does not support two-way binding.");
    }
}