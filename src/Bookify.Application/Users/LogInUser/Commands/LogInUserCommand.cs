using Bookify.Application.Abstractions.Messaging.Commands;
using Bookify.Application.Users.LogInUser.Responses;

namespace Bookify.Application.Users.LogInUser.Commands;

public record LogInUserCommand(
    string Email,
    string Password)
    : ICommand<AccessTokenResponse>;