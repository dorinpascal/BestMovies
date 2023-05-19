using System.Security.Claims;
using BestMovies.Bff.Authorization;
using BestMovies.Shared.Dtos.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BestMovies.Bff.Helpers;

public static class AuthenticationHelpers
{
    public static bool AuthenticateUser(HttpRequest req, out CreateUserDto? user)
    {
        var claims = req.RetrieveClaimsPrincipal();
        var userId = claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (claims.Identity is null || !claims.Identity.IsAuthenticated || string.IsNullOrEmpty(userId))
        {
            {
                user = null;
                return false;
            }
        }

        user = new CreateUserDto(
            Id: userId,
            Email: claims.Identity!.Name!
        );
        
        return true;
    }
}