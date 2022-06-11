using Microsoft.Extensions.DependencyInjection;

namespace FluentChaining;

internal class TypeFinalLinkBuilder<TRequest, TResponse, TFinalLink, TContext> :
    FinalLinkBuilderBase<TRequest, TContext, TResponse>
    where TFinalLink : class, IFinalLinkBase<TRequest, TContext, TResponse>
{
    public TypeFinalLinkBuilder(ILinkBuilder<TRequest, TContext, TResponse> previous) : base(previous) { }

    protected override void InitializeProtected(IServiceCollection collection)
    {
        collection.AddTransient<TFinalLink>();
    }

    protected override LinkDelegate<TRequest, TContext, TResponse> BuildProtected(IServiceProvider provider)
    {
        var instance = provider.GetRequiredService<TFinalLink>();
        return instance.Process;
    }
}