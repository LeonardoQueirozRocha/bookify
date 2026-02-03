using Bookify.Api.Controllers.Users.Requests;
using Bookify.Application.Users.LogInUser.Commands;
using Bookify.Application.Users.RegisterUser.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Api.Controllers.Users;

[ApiController]
[Route("api/users")]
public class UserController(
    ISender sender)
    : ControllerBase
{
    [AllowCookieRedirect]
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