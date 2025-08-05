using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Interfaces;
using velowrench.Services;
using velowrench.ViewModels;

namespace velowrench.Configuration;

public static class ServiceCollectionExtensions
{
    public static void AddCommonServices(this IServiceCollection collection)
    {
        collection.AddSingleton<ILocalizer, Localizer>();
        collection.AddSingleton<MainWindowViewModel>();
    }
}
