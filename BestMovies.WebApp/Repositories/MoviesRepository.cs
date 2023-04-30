using BestMovies.Shared.Dtos.Movies;
using BestMovies.WebApp.Helpers;
using BestMovies.WebApp.Services;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Json;
using System.Text;

namespace BestMovies.WebApp.Repositories;

public class MoviesRepository : IMoviesRepository
{
    private readonly HttpClient _client;
    private readonly IUserProfileService _userProfileService;
    private const string BaseUri = "/api/movies";

    public MoviesRepository(HttpClient client, IUserProfileService userProfileService)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _userProfileService = userProfileService;
    }

    public async Task<IEnumerable<SearchMovieDto>> GetPopularMovies(string? genre)
    {
        var settings = await _userProfileService.GetUserSettings();
        
        var param = new Dictionary<string, string>();
        if (settings.Region is not null) param.Add("region", settings.Region);
        if (settings.Language is not null) param.Add("language", settings.Language);
        if (genre is not null) param.Add("genre", genre);

        var url = QueryHelpers.AddQueryString(BaseUri, param);
        using var response = await _client.GetAsync(url);

        return await SearchMovieDtoResponseHandler(response);
    }

    public async Task<IEnumerable<SearchMovieDto>> SearchMovie(string movieTitle)
    {
        var parameters = new SearchParametersDto(movieTitle);
        string str = JsonSerializer.Serialize(parameters);
        HttpContent content = new StringContent(str, Encoding.UTF8, "application/json");
        HttpResponseMessage responseMessage = await _client.PostAsync($"{BaseUri}/discovery", content);
        
        return await SearchMovieDtoResponseHandler(responseMessage);
    }
    
    private static async Task<IEnumerable<SearchMovieDto>> SearchMovieDtoResponseHandler(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            return Enumerable.Empty<SearchMovieDto>();
        }

        var dtos = await HttpClientHelper.ReadFromJsonSafe<IEnumerable<SearchMovieDto>>(response);
        return dtos ?? Enumerable.Empty<SearchMovieDto>();
    }
}