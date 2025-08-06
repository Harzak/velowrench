using Microsoft.Extensions.DependencyInjection;
using velowrench.Core.Interfaces;
using velowrench.UI.Services;

namespace velowrench.UI.Configuration;

public static class ServiceCollectionExtensions
{
    public static void AddUIServices(this IServiceCollection collection)
    {
        collection.AddSingleton<ILocalizer, Localizer>();
    }
}