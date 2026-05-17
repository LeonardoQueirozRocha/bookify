using System.Net;
using System.Net.Http.Json;
using Bookify.Api.FunctionalTests.Infrastructure;
using Bookify.Application.Users.RegisterUser.Requests;
using FluentAssertions;

namespace Bookify.Api.FunctionalTests.Users;

public class RegisterUserTests(FunctionalTestWebAppFactory factory) : BaseFunctionalTest(factory)
{
    [Fact]
    public async Task Register_ShouldReturnOk_WhenRequestIsValid()
    {
        // Arrange
        var request = new RegisterUserRequest("create@test.com", "first", "last", "12345");

        // Act
        var response = await HttpClient.PostAsJsonAsync("api/v1/users/register", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory]
    [InlineData("", "first", "last", "12345")]
    [InlineData("test.com", "first", "last", "12345")]
    [InlineData("@test.com", "first", "last", "12345")]
    [InlineData("test@", "first", "last", "12345")]
    [InlineData("test@test.com", "", "last", "12345")]
    [InlineData("test@test.com", "first", "", "12345")]
    [InlineData("test@test.com", "first", "", "")]
    [InlineData("test@test.com", "first", "", "1")]
    [InlineData("test@test.com", "first", "", "12")]
    [InlineData("test@test.com", "first", "", "123")]
    [InlineData("test@test.com", "first", "", "1234")]
    public async Task Register_ShouldReturnBadRequest_WhenRequestIsInvalid(
        string email,
        string firstName,
        string lastName,
        string password)
    {
        // Arrange
        var request = new RegisterUserRequest(email, firstName, lastName, password);

        // Act
        var response = await HttpClient.PostAsJsonAsync("api/v1/users/register", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}