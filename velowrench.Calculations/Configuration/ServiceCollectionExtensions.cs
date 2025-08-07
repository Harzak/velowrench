using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Calculations.Calculs.Transmission.ChainLength;
using velowrench.Calculations.Interfaces;
using velowrench.Utils.Interfaces;

namespace velowrench.Calculations.Configuration;

    public static class ServiceCollectionExtensions
    {
        public static void AddCalculationServices(this IServiceCollection collection)
        {
            collection.AddSingleton<ICalculFactory<ChainLengthCalculInput, ChainLengthCalculResult>, ChainLengthCalculFactory>();
        }
    }
