namespace FluentChaining;

internal class ResponsibilityChain<TRequest, TResponse> : IChain<TRequest, TResponse>
{
    private readonly LinkDelegate<TRequest, SynchronousContext, TResponse> _func;

    public ResponsibilityChain(LinkDelegate<TRequest, SynchronousContext, TResponse> func)
    {
        _func = func;
    }

    public TResponse Process(TRequest request)
        => _func.Invoke(request, new SynchronousContext());
}

internal class ResponsibilityChain<TRequest> : IChain<TRequest>
{
    private readonly LinkDelegate<TRequest, SynchronousContext, Unit> _func;

    public ResponsibilityChain(LinkDelegate<TRequest, SynchronousContext, Unit> func)
    {
        _func = func;
    }

    public Unit Process(TRequest request)
        => _func.Invoke(request, new SynchronousContext());
}