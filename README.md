# FluentChaining [![NuGet](https://img.shields.io/nuget/v/FluentChaining?style=flat-square)](https://www.nuget.org/packages/FluentChaining/)

A lightweight library for implementing responsibility chains in fluent style.

## Prerequisites

Chain of responsibility is on OOP pattern outlined by GoF.

> Avoid coupling the sender of a request to its receiver by giving more than one object a chance to handle the request.
> Chain the receiving
> objects and pass the request along the chain until an object handles it.
>
> –– GoF

You can learn more about that pattern on [refactoring.guru](https://refactoring.guru/design-patterns/chain-of-responsibility) website.

## Examples

### Creating an `IChain` (`IAsyncChain`)

You can create an IChain by using `FluentChaining.CreateChain<TRequest, TResponse>` method
or `FluentChaining.CreateChain<TRequest>` for chains that returns no value (`Unit`).

If your chain links have some dependencies to resolve, you can pass down an instance
of `IEnumerable<ServiceDescriptor>`, or simply an `IServiceCollection` that contains these dependencies.

Overloads for chains with `async` links are available as well.

```csharp
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
```

### Registering an `IChain` (`IAsyncChain`) in the `IServiceCollection`

You can also register you chains in DI container using extension methods.

Call `AddFluentChaining` method on `IServiceCollection` to start registration
and configure chain creation processes (optionally).

#### Configuration options

- ChainLifetime \
  The lifetime which the created chains will be registered with (`Scoped` by default). Configuring lifetime for
  individual chains is not supported, yet nothing stops you from calling `AddFluentChaining` multiple times.
- AllowDuplicates \
  If set to `true`, multiple chains with same (`TRequest`, `TResponse`) pair will be allowed, otherwise only the first
  one will be used.

The `AddFluentChaining` method will return `IChainConfigurator` instance, which has methods
for adding all types of chains.

```csharp
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
```

## Usage

To create a chain link, you simply need to implement an `ILink` interface or `IAsyncLink`interface (for async chains).

Links added in the chain support DI (as long as corresponding `IServiceProvider` can satisfy their dependencies).

To process a request by the chain you will need to call a `Process` method on the `IChain` or `ProcessAsync`
on `IAsyncChain` instance.

### `ILink` implementation example

```csharp
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
```

### `IAsyncLink` implementation example

```csharp
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
```

### Chain processing example

```csharp
 public static bool Validate(User user, IServiceProvider provider)
{
    var nameValidator = provider.GetRequiredService<IChain<Name, bool>>();
    var ageValidator = provider.GetRequiredService<IAsyncChain<Age, bool>>();

    return nameValidator.Process(user.Name) && ageValidator.ProcessAsync(user.Age).Result;
}
```

## Utilising assembly scanning

Configuring links manually is definitely is a more reliable and stable approach. On the other hand it is more verbose,
so for the sake
of automation, or in case the link order is not important, you can use assembly scanning.

To add links from selected assemblies call `ThenFromAssemblies` method on your `ILinkBuilder<,,>` instance.
It receives `AssemblyProvider`s as `params`, it will implicitly convert instances of `Type` and `Assembly` objects,
courtesy of [FluentScanning](https://github.com/ronimizy/FluentScanning).

If you want to have an ease of automation that assembly scanning offers, and still have some control over the order of
links in chain,
you can use `LinkPriorityAttribute`. Links will be sorted in descending order by their priority (the higher the
priority,
the earlier the link will be added), with priority 0 for ones without `LinkPriorityAttribute`.

### Assembly scanning example

```csharp
.AddChain<Name, bool>
(
    start => start
        .ThenFromAssemblies(typeof(Scenario))
        .FinishWith(() => true)
)
```

### LinkPriorityAttribute example

```csharp
[LinkPriority(int.MaxValue)]
public class LengthChecker : ILink<Name, bool>
{
    ...
}
```
