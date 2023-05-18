using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BestMovies.Bff.Extensions;
using BestMovies.Shared.Dtos.Movies;

namespace BestMovies.Bff.Clients;

public partial class BestMoviesApiClient
{
    public async Task SaveMovie(string userId, SavedMovieDto savedMovie)
    {
        var savedMovieJson = JsonSerializer.Serialize(savedMovie);
        var savedMovieStringContent = new StringContent(
            savedMovieJson,
            Encoding.UTF8,
            "application/json"
        );
        
        var responseMessage = await _client.PostAsync($"users/{userId}/savedMovies", savedMovieStringContent);
        if (!responseMessage.IsSuccessStatusCode)
        {
            await responseMessage.ThrowBasedOnStatusCode();
        }
    }
}