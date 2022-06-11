namespace FluentChaining;

internal class DelegateFinalLinkBuilder<TRequest, TContext, TResponse> :
    FinalLinkBuilderBase<TRequest, TContext, TResponse>
{
    private readonly LinkDelegate<TRequest, TContext, TResponse> _final;

    public DelegateFinalLinkBuilder(
        LinkDelegate<TRequest, TContext, TResponse> final,
        ILinkBuilder<TRequest, TContext, TResponse> previous) : base(previous)
    {
        _final = final;
    }

    protected override LinkDelegate<TRequest, TContext, TResponse> BuildProtected(IServiceProvider provider)
        => _final;
}