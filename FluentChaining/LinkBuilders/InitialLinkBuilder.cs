namespace FluentChaining;

internal class InitialLinkBuilder<TRequest, TContext, TResponse> : LinkBuilderBase<TRequest, TContext, TResponse>
{
    public override LinkDelegate<TRequest, TContext, TResponse> Build(
        IServiceProvider provider,
        LinkDelegate<TRequest, TContext, TResponse> next)
        => next;
}