using Bookify.Application.Abstractions.Authentication;
using Bookify.Infrastructure.Authentication.Extensions;
using Microsoft.AspNetCore.Http;

namespace Bookify.Infrastructure.Authentication.Context;

internal sealed class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public string IdentityId =>
        httpContextAccessor
            .HttpContext?
            .User
            .GetIdentityId() ??
        throw new ApplicationException("User context is unavailable");
}