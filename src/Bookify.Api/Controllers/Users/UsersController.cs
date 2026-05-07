using Asp.Versioning;
using Bookify.Application.Users.GetLoggedInUser.Queries;
using Bookify.Application.Users.LogInUser.Commands;
using Bookify.Application.Users.LogInUser.Requests;
using Bookify.Application.Users.RegisterUser.Commands;
using Bookify.Application.Users.RegisterUser.Requests;
using Bookify.Infrastructure.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Api.Controllers.Users;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route("api/v{version:apiVersion}/users")]
public class UsersController(ISender sender) : ControllerBase
{
    [HttpGet("me")]
    [HasPermission(Permissions.UserRead)]
    public async Task<IActionResult> GetLoggedInUser(CancellationToken cancellationToken)
    {
        var query = new GetLoggedInUserQuery();

        var result = await sender.Send(query, cancellationToken);

        return Ok(result.Value);
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync(
        RegisterUserRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RegisterUserCommand(
            request.Email,
            request.FirstName,
            request.LastName,
            request.Password);

        var result = await sender.Send(command, cancellationToken);

        return result.IsFailure
            ? BadRequest(result.Error)
            : Ok(result.Value);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> LogInAsync(
        LogInUserRequest request,
        CancellationToken cancellationToken)
    {
        var command = new LogInUserCommand(request.Email, request.Password);
        var result = await sender.Send(command, cancellationToken);

        return result.IsFailure
            ? Unauthorized(result.Error)
            : Ok(result.Value);
    }
}