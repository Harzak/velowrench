using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Repository.Interfaces;
using velowrench.Repository.Repositories;

namespace velowrench.Repository.Configuration;

/// <summary>
/// Extension methods for configuring repository services in the dependency injection container.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds repository application services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    public static void AddRepositoryServices(this IServiceCollection collection)
    {
        collection.AddSingleton<IComponentStandardRepository, ComponentStandardRepository>();
    }
}
