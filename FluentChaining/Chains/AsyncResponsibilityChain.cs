namespace FluentChaining;

internal class AsyncResponsibilityChain<TRequest, TResponse> : IAsyncChain<TRequest, TResponse>
{
    private readonly LinkDelegate<TRequest, AsynchronousContext, Task<TResponse>> _func;

    public AsyncResponsibilityChain(LinkDelegate<TRequest, AsynchronousContext, Task<TResponse>> func)
    {
        _func = func;
    }

    public Task<TResponse> ProcessAsync(TRequest request, CancellationToken cancellationToken)
        => _func.Invoke(request, new AsynchronousContext(cancellationToken));

    Task<TResponse> IAsyncChain<TRequest, TResponse>.ProcessAsync(TRequest request)
        => ProcessAsync(request, CancellationToken.None);
}

internal class AsyncResponsibilityChain<TRequest> : IAsyncChain<TRequest>
{
    private readonly LinkDelegate<TRequest, AsynchronousContext, Task<Unit>> _func;

    public AsyncResponsibilityChain(LinkDelegate<TRequest, AsynchronousContext, Task<Unit>> func)
    {
        _func = func;
    }

    public Task<Unit> ProcessAsync(TRequest request, CancellationToken cancellationToken)
        => _func.Invoke(request, new AsynchronousContext(cancellationToken));

    Task<Unit> IAsyncChain<TRequest, Unit>.ProcessAsync(TRequest request)
        => ProcessAsync(request, CancellationToken.None);
}