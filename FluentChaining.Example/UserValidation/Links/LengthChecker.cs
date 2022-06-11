using FluentChaining.Example.UserValidation.Models;
using FluentChaining.Example.UserValidation.Tools;

namespace FluentChaining.Example.UserValidation.Links;

[LinkPriority(int.MaxValue)]
public class LengthChecker : ILink<Name, bool>
{
    private readonly ValidationConfiguration _configuration;

    public LengthChecker(ValidationConfiguration configuration)
    {
        _configuration = configuration;
    }

    public bool Process(Name request, SynchronousContext context, LinkDelegate<Name, SynchronousContext, bool> next)
    {
        if (request.Value.Length < _configuration.NameLength)
            return false;

        return next.Invoke(request, context);
    }
}