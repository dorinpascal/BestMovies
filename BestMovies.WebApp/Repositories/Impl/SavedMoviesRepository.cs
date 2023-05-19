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

        Console.WriteLine(await response.Content.ReadAsStringAsync());
        if (!response.IsSuccessStatusCode)
        {
            throw new ApiException(await HttpClientHelper.ReadContentSafe(response), (int)response.StatusCode);
        }
    }

    public async Task<IEnumerable<SavedMovieDto>> GetSavedMovies()
    {
        using var response = await _client.GetAsync(BaseUri);
        
        if (!response.IsSuccessStatusCode)
        {
            return Enumerable.Empty<SavedMovieDto>();
        }

        var savedMovies = await HttpClientHelper.ReadFromJsonSafe<IEnumerable<SavedMovieDto>>(response);
        return savedMovies ?? Enumerable.Empty<SavedMovieDto>();
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