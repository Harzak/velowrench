using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Core.Validation.Pipeline;

namespace velowrench.Calculations.Interfaces;

internal interface ICalculatorValidationBuilder<TInput> where TInput : class
{
    ValidationPipeline<TInput> Build();
}