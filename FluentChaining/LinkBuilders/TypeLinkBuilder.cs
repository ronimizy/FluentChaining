using Microsoft.Extensions.DependencyInjection;

namespace FluentChaining;

internal class TypeLinkBuilder<TRequest, TContext, TResponse> : MiddleLinkBuilderBase<TRequest, TContext, TResponse>
{
    private readonly Type _type;

    public TypeLinkBuilder(Type type, ILinkBuilder<TRequest, TContext, TResponse> previous) : base(previous)
    {
        _type = type;
    }

    protected override void InitializeProtected(IServiceCollection collection)
    {
        collection.AddTransient(_type);
    }

    protected override LinkDelegate<TRequest, TContext, TResponse> BuildProtected(
        IServiceProvider provider,
        LinkDelegate<TRequest, TContext, TResponse> next)
    {
        var instance = (ILinkBase<TRequest, TContext, TResponse>)provider.GetRequiredService(_type);
        return (r, c) => instance.Process(r, c, next);
    }
}