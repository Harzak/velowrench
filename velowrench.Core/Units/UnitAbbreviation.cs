using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitsNet;

namespace velowrench.Core.Units;

public struct UnitAbbreviation<T> where T : Enum
{
    public string Abbreviation => this.GetDefaultAbbreviation(this.Type) ?? "N/A";
    public T Type { get; set; }

    public UnitAbbreviation(T type)
    {
        Type = type;
    }

    public string GetDefaultAbbreviation(Enum value)
    {
        return UnitsNetSetup.Default.UnitAbbreviations.GetDefaultAbbreviation(value, CultureInfo.CurrentCulture);
    }
}
