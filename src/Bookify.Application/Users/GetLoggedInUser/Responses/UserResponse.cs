namespace Bookify.Application.Users.GetLoggedInUser.Responses;

public sealed class UserResponse
{
    public Guid Id { get; init; }

    public string? Email { get; init; }

    public string? Name { get; init; }
}