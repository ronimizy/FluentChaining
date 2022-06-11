using FluentChaining.Example.UserValidation.Models;
using FluentChaining.Example.UserValidation.Tools;

namespace FluentChaining.Example.UserValidation.Links;

public class AgeChecker : IAsyncLink<Age, bool>
{
    private readonly ValidationConfiguration _configuration;

    public AgeChecker(ValidationConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task<bool> Process(Age request, AsynchronousContext context, LinkDelegate<Age, AsynchronousContext, Task<bool>> next)
    {
        if (request.Value < _configuration.Age || context.CancellationToken.IsCancellationRequested)
            return Task.FromResult(false);

        return next.Invoke(request, context);
    }
}