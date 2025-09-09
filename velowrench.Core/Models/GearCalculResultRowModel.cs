using CommunityToolkit.Mvvm.ComponentModel;
using UnitsNet;
using velowrench.Calculations.Enums;

namespace velowrench.Core.Models;

/// <summary>
/// Represents a single row of gear calculation results for display purposes.
/// Contains calculated values for a specific sprocket size across all available chainrings.
/// </summary>
public partial class GearCalculResultRowModel : ObservableObject
{
    /// <summary>
    /// Gets or sets the number of teeth on the sprocket for this calculation row.
    /// This value identifies which sprocket size this row represents.
    /// </summary>
    [ObservableProperty]
    private int _sprocketCount;

    /// <summary>
    /// Gets or sets the calculated value for the primary (largest or only) chainring.
    /// Always contains a value as the primary chainring is required for all calculations.
    /// </summary>
    [ObservableProperty]
    private double _valueForChainring1;

    /// <summary>
    /// Gets or sets the calculated value for the medium chainring.
    /// Contains a value only when a medium chainring is configured in the input.
    /// </summary>
    [ObservableProperty]
    private double? _valueForChainring2;

    /// <summary>
    /// Gets or sets the calculated value for the smallest chainring.
    /// Contains a value only when a small chainring is configured in the input.
    /// </summary>
    [ObservableProperty]
    private double? _valueForChainring3;

    [ObservableProperty]
    private EGearCalculationResultIntensity _intensity;

    /// <summary>
    /// Gets or sets the unit of measurement for <see cref="ValueForChainring1"/>, <see cref="ValueForChainring2"/>, <see cref="ValueForChainring3"/>.
    /// </summary>
    [ObservableProperty]
    private Enum? _valueUnit;

    public GearCalculResultRowModel(double valueForChainring1, Enum? valueUnit)
    {
        _valueForChainring1 = valueForChainring1;
        _valueUnit = valueUnit;
    }

    partial void OnValueUnitChanged(Enum? oldValue, Enum? newValue)
    {
        if (oldValue != null && newValue != null && oldValue != newValue)
        {
            TryConvertValues(oldValue, newValue);
        }
    }

    private bool TryConvertValues(Enum oldUnit, Enum newUnit)
    {
        if (!TryConvertValue(this.ValueForChainring1, oldUnit, newUnit, out double convertedValue))
        {
            return false;
        }
        ValueForChainring1 = convertedValue;

        if (this.ValueForChainring2.HasValue)
        {
            if (!TryConvertValue(this.ValueForChainring2.Value, oldUnit, newUnit, out double convertedValue2))
            {
                return false;
            }
            ValueForChainring2 = convertedValue2;
        }

        if (this.ValueForChainring3.HasValue)
        {
            if (!TryConvertValue(this.ValueForChainring3.Value, oldUnit, newUnit, out double convertedValue3))
            {
                return false;
            }
            ValueForChainring3 = convertedValue3;
        }

        return true;
    }

    private bool TryConvertValue(double value, Enum oldUnit, Enum newUnit, out double convertedValue)
    {
        return UnitConverter.TryConvert(value, oldUnit, newUnit, out convertedValue);
    }
}
