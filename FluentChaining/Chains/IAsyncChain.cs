namespace FluentChaining;

public interface IAsyncChain<in TRequest, TResponse>
{
    Task<TResponse> ProcessAsync(TRequest request, CancellationToken cancellationToken);
    Task<TResponse> ProcessAsync(TRequest request);
}

public interface IAsyncChain<in TRequest> : IAsyncChain<TRequest, Unit> { }