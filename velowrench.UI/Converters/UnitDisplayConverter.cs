using Avalonia.Data.Converters;
using System;
using System.Globalization;
using UnitsNet;
using UnitsNet.Units;

namespace velowrench.UI.Converters;

public class UnitDisplayConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is LengthUnit lengthUnit)
        {
            return lengthUnit switch
            {
                LengthUnit.Millimeter => "mm",
                LengthUnit.Centimeter => "cm",
                _ => lengthUnit.ToString()
            };
        }
        
        return value?.ToString();
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}