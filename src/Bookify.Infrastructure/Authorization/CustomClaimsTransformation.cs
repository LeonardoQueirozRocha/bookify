using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Bookify.Infrastructure.Authentication.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Bookify.Infrastructure.Authorization;

public sealed class CustomClaimsTransformation(IServiceProvider serviceProvider) : IClaimsTransformation
{
    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        if (principal.HasClaim(claim => claim.Type is ClaimTypes.Role) &&
            principal.HasClaim(claim => claim.Type is JwtRegisteredClaimNames.Sub))
        {
            return principal;
        }

        using var scope = serviceProvider.CreateScope();
        var authorizationService = scope.ServiceProvider.GetRequiredService<AuthorizationService>();
        var identityId = principal.GetIdentityId();

        var userRoles = await authorizationService.GetRolesForUserAsync(identityId);

        var claimsIdentity = new ClaimsIdentity();

        claimsIdentity.AddClaim(
            new Claim(
                JwtRegisteredClaimNames.Sub,
                userRoles.Id.ToString()));

        userRoles.Roles.ForEach(role =>
            claimsIdentity.AddClaim(
                new Claim(
                    ClaimTypes.Role,
                    role.Name)));

        principal.AddIdentity(claimsIdentity);

        return principal;
    }
}