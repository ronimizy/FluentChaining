using System.Reflection;
using FluentScanning;
using Microsoft.Extensions.DependencyInjection;

namespace FluentChaining;

internal abstract class LinkBuilderBase<TRequest, TContext, TResponse> : ILinkBuilder<TRequest, TContext, TResponse>
{
    public virtual void Initialize(IServiceCollection collection) { }

    public abstract LinkDelegate<TRequest, TContext, TResponse> Build(
        IServiceProvider provider,
        LinkDelegate<TRequest, TContext, TResponse> next);

    public ILinkBuilder<TRequest, TContext, TResponse> Then<TLink>()
        where TLink : class, ILinkBase<TRequest, TContext, TResponse>
    {
        return new TypeLinkBuilder<TRequest, TContext, TResponse>(typeof(TLink), this);
    }

    public ILinkBuilder<TRequest, TContext, TResponse> Then<TLink>(TLink link)
        where TLink : class, ILinkBase<TRequest, TContext, TResponse>
    {
        return new InstanceLinkBuilder<TRequest, TResponse, TLink, TContext>(link, this);
    }

    public ILinkBuilder<TRequest, TContext, TResponse> ThenFromAssemblies(params AssemblyProvider[] providers)
        => ThenFromAssemblies(q => q, providers);

    public ILinkBuilder<TRequest, TContext, TResponse> ThenFromAssemblies(
        Func<IScanningQuery, IScanningQuery> filter,
        params AssemblyProvider[] providers)
    {
        var scanner = new AssemblyScanner(providers);
        var linkType = typeof(ILink<TRequest, TResponse>);

        var query = scanner.ScanForTypesThat()
            .MustBeAssignableTo(linkType)
            .AreNotInterfaces()
            .AreNotAbstractClasses();

        query = filter.Invoke(query);

        IOrderedEnumerable<TypeInfo> types = query.OrderByDescending(ExtractPriority);

        return types.Aggregate(this,
            (current, type) => new TypeLinkBuilder<TRequest, TContext, TResponse>(type, current));
    }

    public IFinalLinkBuilder<TRequest, TContext, TResponse> FinishWith(
        LinkDelegate<TRequest, TContext, TResponse> final)
    {
        return new DelegateFinalLinkBuilder<TRequest, TContext, TResponse>(final, this);
    }

    public IFinalLinkBuilder<TRequest, TContext, TResponse> FinishWith(Func<TResponse> final)
        => FinishWith((_, _) => final.Invoke());

    public IFinalLinkBuilder<TRequest, TContext, TResponse> FinishWith<TFinalLink>()
        where TFinalLink : class, IFinalLinkBase<TRequest, TContext, TResponse>
    {
        return new TypeFinalLinkBuilder<TRequest, TResponse, TFinalLink, TContext>(this);
    }

    public IFinalLinkBuilder<TRequest, TContext, TResponse> FinishWith<TFinalLink>(TFinalLink final)
        where TFinalLink : class, IFinalLinkBase<TRequest, TContext, TResponse>
    {
        return new InstanceFinalLinkBuilder<TRequest, TResponse, TFinalLink, TContext>(final, this);
    }

    private static int ExtractPriority(MemberInfo type)
    {
        var attribute = type.GetCustomAttribute<LinkPriorityAttribute>();
        return attribute?.Value ?? 0;
    }
}