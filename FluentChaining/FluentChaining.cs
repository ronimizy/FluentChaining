using FluentChaining.Configurators;
using Microsoft.Extensions.DependencyInjection;

namespace FluentChaining;

public static class FluentChaining
{
    public static IChain<TRequest, TResponse> CreateChain<TRequest, TResponse>(
        IChainConfigurator.Factory<TRequest, SynchronousContext, TResponse> factory)
    {
        return CreateChain(Enumerable.Empty<ServiceDescriptor>(), factory);
    }

    public static IChain<TRequest, TResponse> CreateChain<TRequest, TResponse>(
        IEnumerable<ServiceDescriptor> initialServices,
        IChainConfigurator.Factory<TRequest, SynchronousContext, TResponse> factory)
    {
        var collection = new ServiceCollection().AddRange(initialServices);
        collection.AddFluentChaining().AddChain<TRequest, TResponse>(factory.Invoke);

        var provider = collection.BuildServiceProvider();
        return provider.GetRequiredService<IChain<TRequest, TResponse>>();
    }

    public static IChain<TRequest> CreateChain<TRequest>(
        IChainConfigurator.Factory<TRequest, SynchronousContext, Unit> factory)
    {
        return CreateChain(Enumerable.Empty<ServiceDescriptor>(), factory);
    }

    public static IChain<TRequest> CreateChain<TRequest>(
        IEnumerable<ServiceDescriptor> initialServices,
        IChainConfigurator.Factory<TRequest, SynchronousContext, Unit> factory)
    {
        var collection = new ServiceCollection().AddRange(initialServices);
        collection.AddFluentChaining().AddChain<TRequest>(factory.Invoke);

        var provider = collection.BuildServiceProvider();
        return provider.GetRequiredService<IChain<TRequest>>();
    }

    public static IAsyncChain<TRequest, TResponse> CreateAsyncChain<TRequest, TResponse>(
        IChainConfigurator.Factory<TRequest, AsynchronousContext, Task<TResponse>> factory)
    {
        return CreateAsyncChain(Enumerable.Empty<ServiceDescriptor>(), factory);
    }

    public static IAsyncChain<TRequest, TResponse> CreateAsyncChain<TRequest, TResponse>(
        IEnumerable<ServiceDescriptor> initialServices,
        IChainConfigurator.Factory<TRequest, AsynchronousContext, Task<TResponse>> factory)
    {
        var collection = new ServiceCollection().AddRange(initialServices);
        collection.AddFluentChaining().AddAsyncChain<TRequest, TResponse>(factory.Invoke);

        var provider = collection.BuildServiceProvider();
        return provider.GetRequiredService<IAsyncChain<TRequest, TResponse>>();
    }

    public static IAsyncChain<TRequest> CreateAsyncChain<TRequest>(
        IChainConfigurator.Factory<TRequest, AsynchronousContext, Task<Unit>> factory)
    {
        return CreateAsyncChain(Enumerable.Empty<ServiceDescriptor>(), factory);
    }

    public static IAsyncChain<TRequest> CreateAsyncChain<TRequest>(
        IEnumerable<ServiceDescriptor> initialServices,
        IChainConfigurator.Factory<TRequest, AsynchronousContext, Task<Unit>> factory)
    {
        var collection = new ServiceCollection().AddRange(initialServices);
        collection.AddFluentChaining().AddAsyncChain<TRequest>(factory.Invoke);

        var provider = collection.BuildServiceProvider();
        return provider.GetRequiredService<IAsyncChain<TRequest>>();
    }
}