using Avalonia.Data.Converters;
using System;
using System.Globalization;
using velowrench.Calculations.Enums;

namespace velowrench.UI.Converters;

public class GearIntensityToColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is EGearCalculationResultIntensity)
        {
            switch (value)
            {
                case EGearCalculationResultIntensity.Low:
                    return "#3BB346";
                case EGearCalculationResultIntensity.Medium:
                    return "#0064FA";
                case EGearCalculationResultIntensity.High:
                    return "#F93920";
                default:
                    return false;
            }
        }
        return "#FFFFFF";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException("GearIntensityToColorConverter does not support two-way binding.");
    }
}
