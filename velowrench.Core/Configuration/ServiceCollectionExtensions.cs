using Microsoft.Extensions.DependencyInjection;
using velowrench.Core.Factories;
using velowrench.Core.Interfaces;
using velowrench.Core.Services;
using velowrench.Core.ViewModels;
using velowrench.Repository.Configuration;

namespace velowrench.Core.Configuration;

/// <summary>
/// Extension methods for configuring core services in the dependency injection container.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds core application services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <remarks>
    /// This method registers essential services including the main view model, navigation service,
    /// and view model factory required for the core application functionality.
    /// </remarks>
    public static void AddCoreServices(this IServiceCollection collection)
    {
        collection.AddRepositoryServices();

        collection.AddSingleton<MainViewModel>();
        collection.AddSingleton<INavigationService, NavigationService>();
        collection.AddSingleton<IViewModelFactory, ViewModelFactory>();
        collection.AddSingleton<IDebounceActionFactory, DebounceActionFactory>();
    }
}