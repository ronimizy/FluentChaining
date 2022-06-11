using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace FluentChaining.Configurators;

public class ChainConfigurator : IChainConfigurator
{
    private readonly IServiceCollection _collection;
    private readonly FluentChainingOptions _options;

    public ChainConfigurator(IServiceCollection collection, FluentChainingOptions options)
    {
        _collection = collection;
        _options = options;
    }

    public IChainConfigurator AddChain<TRequest, TResponse>(
        IChainConfigurator.Factory<TRequest, SynchronousContext, TResponse> factory)
    {
        Add<TRequest, SynchronousContext, TResponse, IChain<TRequest, TResponse>>(
            factory, f => new ResponsibilityChain<TRequest, TResponse>(f));

        return this;
    }

    public IChainConfigurator AddChain<TRequest>(IChainConfigurator.Factory<TRequest, SynchronousContext, Unit> factory)
    {
        Add<TRequest, SynchronousContext, Unit, IChain<TRequest>>(
            factory, f => new ResponsibilityChain<TRequest>(f));

        return this;
    }

    public IChainConfigurator AddAsyncChain<TRequest, TResponse>(
        IChainConfigurator.Factory<TRequest, AsynchronousContext, Task<TResponse>> factory)
    {
        Add<TRequest, AsynchronousContext, Task<TResponse>, IAsyncChain<TRequest, TResponse>>(
            factory, f => new AsyncResponsibilityChain<TRequest, TResponse>(f));

        return this;
    }

    public IChainConfigurator AddAsyncChain<TRequest>(
        IChainConfigurator.Factory<TRequest, AsynchronousContext, Task<Unit>> factory)
    {
        Add<TRequest, AsynchronousContext, Task<Unit>, IAsyncChain<TRequest>>(
            factory, f => new AsyncResponsibilityChain<TRequest>(f));

        return this;
    }

    private void Add<TRequest, TContext, TResponse, TChain>(
        IChainConfigurator.Factory<TRequest, TContext, TResponse> factory,
        Func<LinkDelegate<TRequest, TContext, TResponse>, TChain> chainFactory)
        where TChain : notnull
    {
        var initial = new InitialLinkBuilder<TRequest, TContext, TResponse>();
        IFinalLinkBuilder<TRequest, TContext, TResponse> final = factory.Invoke(initial);

        TChain Factory(IServiceProvider provider)
        {
            LinkDelegate<TRequest, TContext, TResponse> func = final.Build(provider);
            return chainFactory.Invoke(func);
        }

        final.Initialize(_collection);

        var descriptor = new ServiceDescriptor
        (
            serviceType: typeof(TChain),
            factory: p => Factory(p),
            lifetime: _options.ChainLifetime
        );

        AddDescriptor(descriptor);
    }

    private void AddDescriptor(ServiceDescriptor descriptor)
    {
        if (_options.AllowDuplicates)
        {
            _collection.Add(descriptor);
        }
        else
        {
            _collection.TryAdd(descriptor);
        }
    }
}