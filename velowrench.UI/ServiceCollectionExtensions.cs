using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Core.Factories;
using velowrench.Core.Interfaces;
using velowrench.Core.ViewModels;

namespace velowrench.UI;

public static class ServiceCollectionExtensions
{
    public static void AddUIServices(this IServiceCollection collection)
    {
        collection.AddSingleton<ILocalizer, Localizer>();
    }
}