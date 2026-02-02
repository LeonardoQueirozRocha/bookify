using Bookify.Application.Abstractions.Messaging.Commands;

namespace Bookify.Application.Users.RegisterUser.Commands;

public record RegisterUserCommand(
    string Email,
    string FirstName,
    string LastName,
    string Password)
    : ICommand<Guid>;