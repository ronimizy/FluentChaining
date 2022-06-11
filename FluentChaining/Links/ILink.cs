namespace FluentChaining;

public delegate TResponse LinkDelegate<in TRequest, in TContext, out TResponse>(TRequest request, TContext context);

public interface ILinkBase<TRequest, TContext, TResponse>
{
    TResponse Process(TRequest request, TContext context, LinkDelegate<TRequest, TContext, TResponse> next);
}

public interface ILink<TRequest, TResponse> :
    ILinkBase<TRequest, SynchronousContext, TResponse> { }

public interface ILink<TRequest> : ILink<TRequest, Unit> { }

public interface IAsyncLink<TRequest, TResponse> :
    ILinkBase<TRequest, AsynchronousContext, Task<TResponse>> { }

public interface IAsyncLink<TRequest> : IAsyncLink<TRequest, Unit> { }