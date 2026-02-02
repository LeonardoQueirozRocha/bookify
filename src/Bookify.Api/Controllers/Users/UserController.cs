using Bookify.Application.Users.RegisterUser.Commands;
using MediatR;
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

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }
}