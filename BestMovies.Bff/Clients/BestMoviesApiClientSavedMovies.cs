using System.Collections.Generic;
using System.Linq;
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

    public async Task UpdateMovie(string userId, SavedMovieDto savedMovie)
    {
        var savedMovieJson = JsonSerializer.Serialize(savedMovie);
        var savedMovieStringContent = new StringContent(
            savedMovieJson,
            Encoding.UTF8,
            "application/json"
        );
        
        var responseMessage = await _client.PatchAsync($"users/{userId}/savedMovies", savedMovieStringContent);
        if (!responseMessage.IsSuccessStatusCode)
        {
            await responseMessage.ThrowBasedOnStatusCode();
        }
    }
    
    public async Task DeleteMovie(string userId, int movieId)
    {
        var responseMessage = await _client.DeleteAsync($"users/{userId}/savedMovies/{movieId}");
        
        if (!responseMessage.IsSuccessStatusCode)
        {
            await responseMessage.ThrowBasedOnStatusCode();
        }
    }
    
    public  async Task<IEnumerable<SavedMovieDto>> GetSavedMoviesForUser(string userId, bool? isWatched)
    {
        var url = $"users/{userId}/savedMovies";

        if (isWatched is not null)
        {
            url += $"?isWatched={isWatched}";
        }
        
        var responseMessage = await _client.GetAsync(url);
        if (!responseMessage.IsSuccessStatusCode)
        {
            await responseMessage.ThrowBasedOnStatusCode();
        }
        var content = await responseMessage.ReadContentSafe();
        
        return JsonSerializer.Deserialize<IList<SavedMovieDto>>(content, _jsonSerializerOptions) ?? Enumerable.Empty<SavedMovieDto>();
    }
    
    public async Task<SavedMovieDto> GetSavedMovie(string userId, int movieId)
    {
        var responseMessage = await _client.GetAsync($"users/{userId}/savedMovies/{movieId}");
        if (!responseMessage.IsSuccessStatusCode)
        {
            await responseMessage.ThrowBasedOnStatusCode();
        }
        var content = await responseMessage.ReadContentSafe();
        
        return JsonSerializer.Deserialize<SavedMovieDto>(content, _jsonSerializerOptions)!;
    }
}