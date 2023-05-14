using System.Net.Http.Json;
using System.Text.Json;
using BestMovies.WebApp.Authorization;

namespace BestMovies.WebApp.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;

    public AuthService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }
    
    public async Task<UserInformation?> RetrieveUserInformation()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        return await _httpClient.GetFromJsonAsync<UserInformation>("/.auth/me", options);
    }
}