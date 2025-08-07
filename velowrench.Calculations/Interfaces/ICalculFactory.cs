using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace velowrench.Calculations.Interfaces;

public interface ICalculFactory<TInput, TResult> where TInput : class where TResult : class
{
    ICalcul<TInput, TResult> CreateCalcul();
}