namespace FluentChaining;

internal class InstanceFinalLinkBuilder<TRequest, TResponse, TFinalLink, TContext> :
    FinalLinkBuilderBase<TRequest, TContext, TResponse>
    where TFinalLink : class, IFinalLinkBase<TRequest, TContext, TResponse>
{
    private readonly TFinalLink _final;

    public InstanceFinalLinkBuilder(TFinalLink final, ILinkBuilder<TRequest, TContext, TResponse> previous) : base(previous)
    {
        _final = final;
    }


    protected override LinkDelegate<TRequest, TContext, TResponse> BuildProtected(IServiceProvider provider)
        => _final.Process;
}