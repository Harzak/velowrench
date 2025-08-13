using Microsoft.Extensions.DependencyInjection;
using velowrench.Calculations.Calculators.Transmission.Chain;
using velowrench.Calculations.Calculators.Transmission.Gear;
using velowrench.Calculations.Interfaces;

namespace velowrench.Calculations.Configuration;

/// <summary>
/// Extension methods for configuring calculation services in the dependency injection container.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds calculation services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <remarks>
    /// This method registers all necessary calculation factories and services for performing calculations.
    /// </remarks>
    public static void AddCalculationServices(this IServiceCollection collection)
    {
        collection.AddSingleton<ICalculatorFactory<ChainLengthCalculatorInput, ChainLengthCalculatorResult>, ChainLengthCalculatorFactory>();
        collection.AddSingleton<ICalculatorFactory<GearCalculatorInput, GearCalculatorResult>, GearCalculatorFactory>();
        collection.AddSingleton<Func<ICalculatorInputValidation<ChainLengthCalculatorInput>>>(() => new ChainLengthCalculatorInputValidator());
        collection.AddSingleton<Func<ICalculatorInputValidation<GearCalculatorInput>>>(() => new GearCalculatorInputValidator());
    }
}
