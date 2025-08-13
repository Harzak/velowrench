using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace velowrench.Calculations.Calculs.Transmission.Gear;

public enum EGearCalculType
{
    [Display(Name = "Gain Ratio")]
    GainRatio = 0,

    [Display(Name = "Gear Inches")]
    GearInches = 1,

    [Display(Name = "Development")]
    Development = 2,

    [Display(Name = "Speed")]
    Speed = 3,
}
