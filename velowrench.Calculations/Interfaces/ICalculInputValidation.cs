using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace velowrench.Calculations.Interfaces;

public interface ICalculInputValidation<TInput>
{
    IEnumerable<string> ErrorMessages { get; }
    bool Validate(TInput input);
}