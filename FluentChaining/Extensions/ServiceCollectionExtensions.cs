using FluentChaining.Configurators;
using Microsoft.Extensions.DependencyInjection;

namespace FluentChaining;

public static class ServiceCollectionExtensions
{
    public static IChainConfigurator AddFluentChaining(
        this IServiceCollection collection,
        Action<FluentChainingOptions>? optionsAction = null)
    {
        var options = new FluentChainingOptions();
        optionsAction?.Invoke(options);

        return new ChainConfigurator(collection, options);
    }

    internal static IServiceCollection AddRange(this IServiceCollection collection, IEnumerable<ServiceDescriptor> descriptors)
    {
        foreach (var descriptor in descriptors)
        {
            collection.Add(descriptor);
        }

        return collection;
    }
}