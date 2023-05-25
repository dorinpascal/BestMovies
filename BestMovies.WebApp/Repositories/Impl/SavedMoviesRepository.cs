using System.Text;
using System.Text.Json;
using BestMovies.Shared.CustomExceptions;
using BestMovies.Shared.Dtos.Movies;
using BestMovies.WebApp.Helpers;

namespace BestMovies.WebApp.Repositories.Impl;

public class SavedMoviesRepository : ISavedMoviesRepository
{
    private readonly HttpClient _client;
    private const string BaseUri = "/api/savedMovies";

    public SavedMoviesRepository(HttpClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }
    
    public async Task SaveMovie(SavedMovieDto movieDto)
    {
        var json = JsonSerializer.Serialize(movieDto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _client.PostAsync(BaseUri, content);

        if (!response.IsSuccessStatusCode)
        {
            throw new ApiException(await HttpClientHelper.ReadContentSafe(response), (int)response.StatusCode);
        }
    }

    public async Task<IEnumerable<SearchMovieDto>> GetSavedMovies(bool? isWatched = null)
    {
        var url = new StringBuilder(BaseUri);

        if (isWatched is not null)
        {
            url.Append($"?isWatched={isWatched.Value}");
        }
        
        using var response = await _client.GetAsync(url.ToString());
        
        if (!response.IsSuccessStatusCode)
        {
            return Enumerable.Empty<SearchMovieDto>();
        }

        var savedMovies = await HttpClientHelper.ReadFromJsonSafe<IEnumerable<SearchMovieDto>>(response);
        return savedMovies ?? Enumerable.Empty<SearchMovieDto>();
    }

    public async Task RemoveMovie(int movieId)
    {
        var response = await _client.DeleteAsync($"{BaseUri}/{movieId}");
        
        if (!response.IsSuccessStatusCode)
        {
            throw new ApiException(await HttpClientHelper.ReadContentSafe(response), (int)response.StatusCode);
        }
    }

    public async Task<SavedMovieDto?> GetSavedMovie(int movieId)
    {
        try
        {
            using var response = await _client.GetAsync($"{BaseUri}/{movieId}");
        
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await HttpClientHelper.ReadFromJsonSafe<SavedMovieDto>(response);
        }
        catch (ApiException)
        {
            return null;
        }
        
    }
}