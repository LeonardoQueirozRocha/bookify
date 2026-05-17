using Bookify.Application.Users.RegisterUser.Requests;

namespace Bookify.Api.FunctionalTests.Users;

internal static class UserData
{
    public static RegisterUserRequest RegisterUserRequest =
        new("test@test.com", "test", "test", "12345");
}