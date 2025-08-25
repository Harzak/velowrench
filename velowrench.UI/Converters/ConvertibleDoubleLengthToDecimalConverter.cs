using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitsNet.Units;
using velowrench.Core.Units;

namespace velowrench.UI.Converters;

public class ConvertibleDoubleLengthToDecimalConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is ConvertibleDouble<LengthUnit> convertible)
        {
            if (convertible.IsEmpty)
            {
                return null;
            }

            return (decimal?)convertible.Value;
        }
        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (targetType == typeof(ConvertibleDouble<LengthUnit>) && value is decimal decimalValue)
        {
            return new ConvertibleDouble<LengthUnit>((double)decimalValue, LengthUnit.Millimeter);
        }
        return new ConvertibleDouble<LengthUnit>(LengthUnit.Millimeter);
    }
}