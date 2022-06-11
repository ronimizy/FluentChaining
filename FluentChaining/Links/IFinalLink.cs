namespace FluentChaining;

public interface IFinalLinkBase<in TRequest, in TContext, out TResponse>
{
    TResponse Process(TRequest request, TContext context);
}

public interface IFinalLink<in TRequest, out TResponse> : IFinalLinkBase<TRequest, SynchronousContext, TResponse> { }

public interface IFinalLink<in TRequest> : IFinalLink<TRequest, Unit> { }

public interface IFinalAsyncLink<in TRequest, out TResponse> : IFinalLinkBase<TRequest, AsynchronousContext, TResponse> { }

public interface IFinalAsyncLink<in TRequest> : IFinalAsyncLink<TRequest, Unit> { }