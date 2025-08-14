using Avalonia.Data.Converters;
using System;
using System.Globalization;
using UnitsNet;
using UnitsNet.Units;
using velowrench.Core.Units;

namespace velowrench.UI.Converters;

public class UnitDisplayConverter : IValueConverter
{

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is LengthUnit lengthUnit)
        {
            UnitAbbreviation<LengthUnit> unitAbbreviation = new((LengthUnit)value);
            return unitAbbreviation.Abbreviation;
        }
        
        return value?.ToString();
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}