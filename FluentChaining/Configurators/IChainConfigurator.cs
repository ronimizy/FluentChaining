namespace FluentChaining.Configurators;

public interface IChainConfigurator
{
    delegate IFinalLinkBuilder<TRequest, TContext, TResponse> Factory<TRequest, TContext, TResponse>(
        ILinkBuilder<TRequest, TContext, TResponse> start);

    IChainConfigurator AddChain<TRequest, TResponse>(Factory<TRequest, SynchronousContext, TResponse> factory);

    IChainConfigurator AddChain<TRequest>(Factory<TRequest, SynchronousContext, Unit> factory);

    IChainConfigurator AddAsyncChain<TRequest, TResponse>(Factory<TRequest, AsynchronousContext, Task<TResponse>> factory);

    IChainConfigurator AddAsyncChain<TRequest>(Factory<TRequest, AsynchronousContext, Task<Unit>> factory);
}