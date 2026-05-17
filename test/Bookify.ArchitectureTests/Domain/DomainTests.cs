using System.Reflection;
using Bookify.ArchitectureTests.Infrastructure;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Abstractions.Interfaces;
using FluentAssertions;
using NetArchTest.Rules;

namespace Bookify.ArchitectureTests.Domain;

public class DomainTests : BaseTest
{
    [Fact]
    public void DomainEvents_Should_BeSealed()
    {
        // Arrange && Act
        var result = Types.InAssembly(DomainAssembly)
            .That()
            .ImplementInterface(typeof(IDomainEvent))
            .Should()
            .BeSealed()
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void DomainEvents_ShouldHave_DomainEventPostfix()
    {
        // Arrange && Act
        var result = Types.InAssembly(DomainAssembly)
            .That()
            .ImplementInterface(typeof(IDomainEvent))
            .Should()
            .HaveNameEndingWith("DomainEvent")
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Entities_ShouldHave_PrivateParameterlessConstructor()
    {
        // Arrange
        var entityTypes = Types.InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(Entity))
            .GetTypes();

        // Act
        var failingTypes = entityTypes
            .Where(entityType =>
            {
                var constructors = entityType.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);
                var hasPrivateParameterlessConstructor =
                    constructors.Any(c => c.IsPrivate && c.GetParameters().Length == 0);

                return !hasPrivateParameterlessConstructor;
            });

        // Assert
        failingTypes.Should().BeEmpty();
    }
}