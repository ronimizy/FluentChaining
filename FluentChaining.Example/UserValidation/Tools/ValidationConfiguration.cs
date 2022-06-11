namespace FluentChaining.Example.UserValidation.Tools;

public record ValidationConfiguration(
    int Age,
    int NameLength,
    int DigitCount,
    int SpecialSymbolsCount,
    IReadOnlyCollection<char> SpecialSymbols);