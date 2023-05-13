namespace BestMovies.WebApp.Authorization;

// ReSharper disable once ClassNeverInstantiated.Global
public class ClientPrincipal
{
    public string IdentityProvider { get; set; } = null!;
    public string UserDetails { get; set; } = null!;
}