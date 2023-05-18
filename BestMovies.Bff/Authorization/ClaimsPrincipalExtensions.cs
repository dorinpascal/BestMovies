using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace BestMovies.Bff.Authorization;

public static class ClaimsPrincipalExtensions
{
    public static ClaimsPrincipal RetrieveClaimsPrincipal(this HttpRequest req)
    {
        ClientPrincipal principal = new();

        try
        {
            if (req.Headers.TryGetValue("x-ms-client-principal", out var header))
            {
                var data = header[0];
                var decoded = Convert.FromBase64String(data);
                var json = Encoding.UTF8.GetString(decoded);
                
                principal = JsonSerializer.Deserialize<ClientPrincipal>(json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }) ?? new ClientPrincipal();
            }

            principal.UserRoles = principal.UserRoles
                .Except(new[] {"anonymous"}, StringComparer.CurrentCultureIgnoreCase)
                .ToArray();

            if (!principal.UserRoles.Any())
            {
                return new ClaimsPrincipal();
            }

            var identity = new ClaimsIdentity(principal.IdentityProvider);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, principal.UserId));
            identity.AddClaim(new Claim(ClaimTypes.Name, principal.UserDetails));
            identity.AddClaims(principal.UserRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            return new ClaimsPrincipal(identity);
        }
        catch (Exception ex)
        {
            throw new ArgumentException(ex.Message);
        }
        
    }
    
    private class ClientPrincipal
    {
        public string IdentityProvider { get; set; } = default!;
        public string UserId { get; set; } = default!;
        public string UserDetails { get; set; } = default!;
        public string[] UserRoles { get; set; } = Array.Empty<string>();
    }
}
