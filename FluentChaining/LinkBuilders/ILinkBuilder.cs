using FluentScanning;
using Microsoft.Extensions.DependencyInjection;

namespace FluentChaining;

public interface ILinkBuilder<TRequest, TContext, TResponse>
{
    void Initialize(IServiceCollection collection);

    LinkDelegate<TRequest, TContext, TResponse> Build(
        IServiceProvider provider,
        LinkDelegate<TRequest, TContext, TResponse> next);

    ILinkBuilder<TRequest, TContext, TResponse> Then<TLink>()
        where TLink : class, ILinkBase<TRequest, TContext, TResponse>;

    ILinkBuilder<TRequest, TContext, TResponse> Then<TLink>(TLink link)
        where TLink : class, ILinkBase<TRequest, TContext, TResponse>;

    ILinkBuilder<TRequest, TContext, TResponse> ThenFromAssemblies(params AssemblyProvider[] providers);

    ILinkBuilder<TRequest, TContext, TResponse> ThenFromAssemblies(
        Func<IScanningQuery, IScanningQuery> filter,
        params AssemblyProvider[] providers);

    IFinalLinkBuilder<TRequest, TContext, TResponse> FinishWith(LinkDelegate<TRequest, TContext, TResponse> final);

    IFinalLinkBuilder<TRequest, TContext, TResponse> FinishWith(Func<TResponse> final);

    IFinalLinkBuilder<TRequest, TContext, TResponse> FinishWith<TFinalLink>()
        where TFinalLink : class, IFinalLinkBase<TRequest, TContext, TResponse>;

    IFinalLinkBuilder<TRequest, TContext, TResponse> FinishWith<TFinalLink>(TFinalLink final)
        where TFinalLink : class, IFinalLinkBase<TRequest, TContext, TResponse>;
}