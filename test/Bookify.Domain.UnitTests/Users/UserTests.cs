using Bookify.Domain.UnitTests.Infrastructure;
using Bookify.Domain.Users.Entities;
using Bookify.Domain.Users.Events;
using FluentAssertions;

namespace Bookify.Domain.UnitTests.Users;

public class UserTests : BaseTest
{
    [Fact(DisplayName = $"{nameof(User)} {nameof(User.Create)} should set property values")]
    public void Create_Should_SetPropertyValues()
    {
        // Arrange && Act
        var user = User.Create(UserData.FirstName, UserData.LastName, UserData.Email);

        // Assert
        user.FirstName.Should().Be(UserData.FirstName);
        user.LastName.Should().Be(UserData.LastName);
        user.Email.Should().Be(UserData.Email);
    }

    [Fact(DisplayName = $"{nameof(User)} {nameof(User.Create)} should raise {nameof(UserCreatedDomainEvent)}")]
    public void Create_Should_RaiseUserCreatedDomainEvent()
    {
        // Arrange && Act
        var user = User.Create(UserData.FirstName, UserData.LastName, UserData.Email);

        // Assert
        var domainEvent = AssertDomainEventWasPublished<UserCreatedDomainEvent>(user);
        domainEvent.UserId.Should().Be(user.Id);
    }

    [Fact(DisplayName = $"{nameof(User)} {nameof(User.Create)} add registered role to user")]
    public void Create_Should_AddRegisteredRoleToUser()
    {
        // Arrange && Act
        var user = User.Create(UserData.FirstName, UserData.LastName, UserData.Email);

        // Assert
        user.Roles.Should().Contain(Role.Registered);
    }
}