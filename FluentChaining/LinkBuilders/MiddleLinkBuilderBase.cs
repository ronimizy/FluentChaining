using Microsoft.Extensions.DependencyInjection;

namespace FluentChaining;

internal abstract class MiddleLinkBuilderBase<TRequest, TContext, TResponse> : LinkBuilderBase<TRequest, TContext, TResponse>
{
    private readonly ILinkBuilder<TRequest, TContext, TResponse> _previous;

    protected MiddleLinkBuilderBase(ILinkBuilder<TRequest, TContext, TResponse> previous)
    {
        _previous = previous;
    }

    public sealed override void Initialize(IServiceCollection collection)
    {
        InitializeProtected(collection);
        _previous.Initialize(collection);
    }

    protected virtual void InitializeProtected(IServiceCollection collection) { }

    public sealed override LinkDelegate<TRequest, TContext, TResponse> Build(
        IServiceProvider provider,
        LinkDelegate<TRequest, TContext, TResponse> next)
    {
        LinkDelegate<TRequest, TContext, TResponse> current = BuildProtected(provider, next);
        return _previous.Build(provider, current);
    }

    protected abstract LinkDelegate<TRequest, TContext, TResponse> BuildProtected(
        IServiceProvider provider,
        LinkDelegate<TRequest, TContext, TResponse> next);
}