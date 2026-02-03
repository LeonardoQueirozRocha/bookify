namespace Bookify.Api.Controllers.Users.Requests;

public record LogInUserRequest(
    string Email,
    string Password);