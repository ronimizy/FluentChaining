namespace FluentChaining;

internal class InstanceLinkBuilder<TRequest, TResponse, TLink, TContext> : MiddleLinkBuilderBase<TRequest, TContext, TResponse>
    where TLink : class, ILinkBase<TRequest, TContext, TResponse>
{
    private readonly TLink _link;

    public InstanceLinkBuilder(TLink link, ILinkBuilder<TRequest, TContext, TResponse> previous) : base(previous)
    {
        _link = link;
    }

    protected override LinkDelegate<TRequest, TContext, TResponse> BuildProtected(
        IServiceProvider provider,
        LinkDelegate<TRequest, TContext, TResponse> next)
    {
        return (r, c) => _link.Process(r, c, next);
    }
}