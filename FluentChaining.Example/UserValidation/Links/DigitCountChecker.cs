using FluentChaining.Example.UserValidation.Models;
using FluentChaining.Example.UserValidation.Tools;

namespace FluentChaining.Example.UserValidation.Links;

public class DigitCountChecker : ILink<Name, bool>
{
    private readonly ValidationConfiguration _configuration;

    public DigitCountChecker(ValidationConfiguration configuration)
    {
        _configuration = configuration;
    }

    public bool Process(Name request, SynchronousContext context, LinkDelegate<Name, SynchronousContext, bool> next)
    {
        if (request.Value.Count(char.IsDigit) < _configuration.DigitCount)
            return false;

        return next.Invoke(request, context);
    }
}