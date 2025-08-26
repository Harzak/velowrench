using Microsoft.Extensions.DependencyInjection;
using velowrench.Calculations.Calculators.Transmission.Chain;
using velowrench.Calculations.Calculators.Transmission.Gear;
using velowrench.Calculations.Calculators.Wheels.SpokeLength;
using velowrench.Calculations.Interfaces;
using velowrench.Calculations.Validation;
using velowrench.Calculations.Validation.Builder;

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
        collection.AddSingleton<ICalculatorValidationBuilder<ChainLengthCalculatorInput>, ChainLengthcalculatorValidationBuilder>();
        collection.AddSingleton<ICalculatorInputValidator<ChainLengthCalculatorInput>, CalulatorInputValidator<ChainLengthCalculatorInput>>();

        collection.AddSingleton<ICalculatorFactory<GearCalculatorInput, GearCalculatorResult>, GearCalculatorFactory>();
        collection.AddSingleton<ICalculatorValidationBuilder<GearCalculatorInput>, GearCalculatorValidationBuilder>();
        collection.AddSingleton<ICalculatorInputValidator<GearCalculatorInput>, CalulatorInputValidator<GearCalculatorInput>>();

        collection.AddSingleton<ICalculatorFactory<SpokeLengthCalculatorInput, SpokeLengthCalculatorResult>, SpokeLengthCalculatorFactory>();
        collection.AddSingleton<ICalculatorValidationBuilder<SpokeLengthCalculatorInput>, SpokeLengthCalculatorValidationBuilder>();
        collection.AddSingleton<ICalculatorInputValidator<SpokeLengthCalculatorInput>, CalulatorInputValidator<SpokeLengthCalculatorInput>>();
    }
}
