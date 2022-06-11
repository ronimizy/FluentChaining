using Microsoft.Extensions.DependencyInjection;

namespace FluentChaining;

public interface IFinalLinkBuilder<in TRequest, in TContext, out TResponse>
{
    void Initialize(IServiceCollection collection);
    LinkDelegate<TRequest, TContext, TResponse> Build(IServiceProvider provider);
}