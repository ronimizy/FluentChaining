using System;
using FluentChaining.Example.UserValidation;
using FluentChaining.Example.UserValidation.Models;
using FluentChaining.Example.UserValidation.Tools;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace FluentChaining.Tests;

public class Tests
{
    private const int ValidAge = 18;
    private const int ValidNameLength = 5;
    private const int ValidDigitCount = 1;
    private const int ValidSpecialCharacterCount = 1;

    private static readonly char[] SpecialCharacters =
    {
        '*', '/', '-', '~', '!'
    };

    private IServiceProvider _provider = null!;

    [SetUp]
    public void Setup()
    {
        var collection = new ServiceCollection();
        var configuration = new ValidationConfiguration(ValidAge, ValidNameLength, ValidDigitCount,
            ValidSpecialCharacterCount, SpecialCharacters);

        collection.AddSingleton(configuration);

        Scenario.AddChainsToServiceCollection(collection);

        _provider = collection.BuildServiceProvider();
    }

    [Test]
    public void ValidUserTest_ReturnsTrue()
    {
        var user = new User(new Name("John1!"), new Age(19));
        var result = Scenario.Validate(user, _provider);
        
        Assert.IsTrue(result);
    }
    
    [Test]
    public void InValidUserTest_ReturnsFalse()
    {
        var user = new User(new Name("John"), new Age(17));
        var result = Scenario.Validate(user, _provider);
        
        Assert.IsFalse(result);
    }
}