namespace Bookify.Application.Users.RegisterUser.Requests;

public sealed record RegisterUserRequest(
    string Email,
    string FirstName,
    string LastName,
    string Password);