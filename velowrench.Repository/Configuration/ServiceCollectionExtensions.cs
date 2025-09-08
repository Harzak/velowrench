using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using velowrench.Repository.Interfaces;
using velowrench.Repository.Models;
using velowrench.Repository.Repositories;
using velowrench.Repository.Services;

namespace velowrench.Repository.Configuration;

/// <summary>
/// Extension methods for configuring repository services in the dependency injection container.
/// </summary>
public static class ServiceCollectionExtensions
{
    private const string APP_NAME = "veloWrench";
    private const string USER_PREFERENCES_FILE = "userPreferences.json";

    /// <summary>
    /// Adds repository application services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    public static void AddRepositoryServices(this IServiceCollection collection)
    {
        collection.AddSingleton<IJsonStorage<UserPreferenceModel>>(provider =>
        {
            var logger = provider.GetRequiredService<ILogger<JsonStorage<UserPreferenceModel>>>();
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string appDirectory = Path.Combine(appDataPath, APP_NAME);
            return new JsonStorage<UserPreferenceModel>(appDirectory, USER_PREFERENCES_FILE, logger);
        });
        collection.AddSingleton<IComponentStandardRepository, ComponentStandardRepository>();
        collection.AddSingleton<IUserPreferenceRepository, UserPreferenceRepository>();
    }
}
