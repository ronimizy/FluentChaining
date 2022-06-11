namespace FluentChaining;

public interface IChain<in TRequest, out TResponse>
{
    TResponse Process(TRequest request);
}

public interface IChain<in TRequest> : IChain<TRequest, Unit> { }