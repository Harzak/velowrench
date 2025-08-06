using Microsoft.Extensions.DependencyInjection;
using velowrench.Core.Factories;
using velowrench.Core.Interfaces;
using velowrench.Core.Services;
using velowrench.Core.ViewModels;

namespace velowrench.Core.Configuration;

public static class ServiceCollectionExtensions
{
    public static void AddCoreServices(this IServiceCollection collection)
    {
        collection.AddSingleton<MainViewModel>();
        collection.AddSingleton<INavigationService, NavigationService>();
        collection.AddSingleton<IViewModelFactory, ViewModelFactory>();
    }
}