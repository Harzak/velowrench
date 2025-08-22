using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitsNet;

namespace velowrench.Calculations.Units;

/// <summary>
/// Represents a unit abbreviation for a specific unit type.
/// </summary>
/// <typeparam name="T">The enum type representing the unit type (e.g., LengthUnit, VolumeUnit).</typeparam>
public struct UnitAbbreviation<T> : IEquatable<UnitAbbreviation<T>> where T : Enum
{
    /// <summary>
    /// Gets the default abbreviation for the current unit type in the current culture.
    /// </summary>
    /// <value>
    /// The localized abbreviation string for the unit, or "N/A" if no abbreviation is available.
    /// </value>
    public readonly string Abbreviation => this.GetDefaultAbbreviation(this.Type) ?? "N/A";

    /// <summary>
    /// Gets or sets the unit type for which to retrieve the abbreviation.
    /// </summary>
    public T Type { get; set; }

    public UnitAbbreviation(T type)
    {
        Type = type;
    }

    /// <summary>
    /// Gets the default abbreviation for the specified unit enum value.
    /// </summary>
    /// <returns>The default abbreviation string for the unit in the current culture.</returns>
    public readonly string GetDefaultAbbreviation(Enum value)
    {
        return UnitsNetSetup.Default.UnitAbbreviations.GetDefaultAbbreviation(value, CultureInfo.CurrentCulture);
    }

    public override readonly bool Equals(object? obj)
    {
        if (obj is UnitAbbreviation<T> other)
        {
            return Equals(other);
        }
        return false;
    }

    public readonly bool Equals(UnitAbbreviation<T> other)
    {
        return EqualityComparer<T>.Default.Equals(Type, other.Type);
    }

    public override readonly int GetHashCode()
    {
        return EqualityComparer<T>.Default.GetHashCode(Type);
    }

    public static bool operator ==(UnitAbbreviation<T> left, UnitAbbreviation<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(UnitAbbreviation<T> left, UnitAbbreviation<T> right)
    {
        return !(left == right);
    }
}
