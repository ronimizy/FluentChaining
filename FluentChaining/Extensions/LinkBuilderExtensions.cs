namespace FluentChaining;

public static class LinkBuilderExtensions
{
    public static IFinalLinkBuilder<TRequest, TContext, Unit> Finish<TRequest, TContext>(
        this ILinkBuilder<TRequest, TContext, Unit> builder)
    {
        return builder.FinishWith((_, _) => Unit.Value);
    }

    public static IFinalLinkBuilder<TRequest, TContext, Task<Unit>> Finish<TRequest, TContext>(
        this ILinkBuilder<TRequest, TContext, Task<Unit>> builder)
    {
        return builder.FinishWith((_, _) => Unit.Task);
    }

    public static IFinalLinkBuilder<TRequest, TContext, Task<TResponse>> FinishFromResult<TRequest, TContext, TResponse>(
        this ILinkBuilder<TRequest, TContext, Task<TResponse>> builder,
        Func<TResponse> factory)
    {
        return builder.FinishWith(() => Task.FromResult(factory.Invoke()));
    }
}