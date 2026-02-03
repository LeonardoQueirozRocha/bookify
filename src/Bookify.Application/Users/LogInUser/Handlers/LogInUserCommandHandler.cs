using Bookify.Application.Abstractions.Authentication;
using Bookify.Application.Abstractions.Messaging.Commands;
using Bookify.Application.Users.LogInUser.Commands;
using Bookify.Application.Users.LogInUser.Responses;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Users.Errors;

namespace Bookify.Application.Users.LogInUser.Handlers;

internal sealed class LogInUserCommandHandler(
    IJwtService jwtService)
    : ICommandHandler<LogInUserCommand, AccessTokenResponse>
{
    public async Task<Result<AccessTokenResponse>> Handle(
        LogInUserCommand request,
        CancellationToken cancellationToken)
    {
        var result = await jwtService.GetAccessTokenAsync(
            request.Email,
            request.Password,
            cancellationToken);

        return result.IsFailure
            ? Result.Failure<AccessTokenResponse>(UserErrors.InvalidCredentials)
            : new AccessTokenResponse(result.Value);
    }
}