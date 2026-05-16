using System.Net.Http.Json;
using Bookify.Api.FunctionalTests.Users;
using Bookify.Application.Users.LogInUser.Requests;
using Bookify.Application.Users.LogInUser.Responses;

namespace Bookify.Api.FunctionalTests.Infrastructure;

public abstract class BaseFunctionalTest(
    FunctionalTestWebAppFactory factory)
    : IClassFixture<FunctionalTestWebAppFactory>
{
    protected readonly HttpClient HttpClient = factory.CreateClient();

    protected async Task<string> GetAccessToken()
    {
        const string requestUri = "api/v1/users/login";

        var request = new LogInUserRequest(
            UserData.RegisterUserRequest.Email,
            UserData.RegisterUserRequest.Password);

        var loginResponse = await HttpClient.PostAsJsonAsync(requestUri, request);

        var accessTokenResponse = await loginResponse.Content.ReadFromJsonAsync<AccessTokenResponse>();

        return accessTokenResponse!.AccessToken;
    }
}