using FluentChaining.Example.UserValidation.Links;
using FluentChaining.Example.UserValidation.Models;
using Microsoft.Extensions.DependencyInjection;

namespace FluentChaining.Example.UserValidation;

public class Scenario
{
    public static IChain<Name, bool> CreateChain(IServiceCollection initialServices)
    {
        return FluentChaining.CreateChain<Name, bool>
        (
            initialServices,
            builder => builder
                .Then<LengthChecker>()
                .Then<DigitCountChecker>()
                .Then<SpecialSymbolsChecker>()
                .FinishWith(() => true)
        );
    }

    public static void AddChainsToServiceCollection(IServiceCollection collection)
    {
        collection.AddFluentChaining(o =>
            {
                o.ChainLifetime = ServiceLifetime.Singleton;
                o.AllowDuplicates = false;
            })
            .AddChain<Name, bool>
            (
                start => start
                    .ThenFromAssemblies(typeof(Scenario))
                    .FinishWith(() => true)
            )
            .AddChain<Name, bool>
            (
                start => start
                    .Then<LengthChecker>()
                    .Then<DigitCountChecker>()
                    .Then<SpecialSymbolsChecker>()
                    .FinishWith(() => true)
            )
            .AddAsyncChain<Age, bool>
            (
                start => start
                    .Then<AgeChecker>()
                    .FinishFromResult(() => true)
            );
    }

    public static bool Validate(User user, IServiceProvider provider)
    {
        var nameValidator = provider.GetRequiredService<IChain<Name, bool>>();
        var ageValidator = provider.GetRequiredService<IAsyncChain<Age, bool>>();

        return nameValidator.Process(user.Name) && ageValidator.ProcessAsync(user.Age).Result;
    }
}