using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Factories;
using velowrench.Interfaces;
using velowrench.Services;
using velowrench.ViewModels;

namespace velowrench.Configuration;

public static class ServiceCollectionExtensions
{
    public static void AddCommonServices(this IServiceCollection collection)
    {
        collection.AddSingleton<MainWindowViewModel>();
        collection.AddSingleton<ILocalizer, Localizer>();
        collection.AddSingleton<IToolsViewModelFactory, ToolsViewModelFactory>();
    }
}