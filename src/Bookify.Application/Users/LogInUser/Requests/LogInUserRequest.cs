namespace Bookify.Application.Users.LogInUser.Requests;

public record LogInUserRequest(
    string Email,
    string Password);