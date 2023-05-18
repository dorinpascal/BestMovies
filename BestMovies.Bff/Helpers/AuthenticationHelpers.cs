using System.Security.Claims;
using BestMovies.Bff.Authorization;
using BestMovies.Shared.Dtos.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BestMovies.Bff.Helpers;

public static class AuthenticationHelpers
{
    public static bool AuthenticateUser(HttpRequest req, out CreateUserDto? user, out IActionResult actionResult)
    {
        var claims = req.RetrieveClaimsPrincipal();
        var userId = claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (claims.Identity is null || !claims.Identity.IsAuthenticated || string.IsNullOrEmpty(userId))
        {
            {
                actionResult = ActionResultHelpers.UnauthorizedResult();
                user = null;
                return true;
            }
        }

        user = new CreateUserDto(
            Id: userId,
            Email: claims.Identity!.Name!
        );
        
        actionResult = default!;
        return false;
    }
}