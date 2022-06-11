using Microsoft.Extensions.DependencyInjection;

namespace FluentChaining;

internal abstract class FinalLinkBuilderBase<TRequest, TContext, TResponse> :
    IFinalLinkBuilder<TRequest, TContext, TResponse>
{
    private readonly ILinkBuilder<TRequest, TContext, TResponse> _previous;

    protected FinalLinkBuilderBase(ILinkBuilder<TRequest, TContext, TResponse> previous)
    {
        _previous = previous;
    }

    public void Initialize(IServiceCollection collection)
    {
        InitializeProtected(collection);
        _previous.Initialize(collection);
    }

    protected virtual void InitializeProtected(IServiceCollection collection) { }

    public LinkDelegate<TRequest, TContext, TResponse> Build(IServiceProvider provider)
    {
        LinkDelegate<TRequest, TContext, TResponse> final = BuildProtected(provider);
        return _previous.Build(provider, final);
    }

    protected abstract LinkDelegate<TRequest, TContext, TResponse> BuildProtected(IServiceProvider provider);
}