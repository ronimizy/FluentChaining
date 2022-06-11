using FluentChaining.Example.UserValidation.Models;
using FluentChaining.Example.UserValidation.Tools;

namespace FluentChaining.Example.UserValidation.Links;

public class SpecialSymbolsChecker : ILink<Name, bool>
{
    private readonly ValidationConfiguration _configuration;

    public SpecialSymbolsChecker(ValidationConfiguration configuration)
    {
        _configuration = configuration;
    }

    public bool Process(Name request, SynchronousContext context, LinkDelegate<Name, SynchronousContext, bool> next)
    {
        if (request.Value.Count(c => _configuration.SpecialSymbols.Contains(c)) < _configuration.SpecialSymbolsCount)
            return false;

        return next(request, context);
    }
}