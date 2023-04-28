using System.Net.Http.Json;
using BestMovies.Shared.Dtos.Movies;
using BestMovies.WebApp.Helpers;
using BestMovies.WebApp.Services;
using Microsoft.AspNetCore.WebUtilities;

namespace BestMovies.WebApp.Repositories;

public class MoviesRepository : IMoviesRepository
{
    private readonly HttpClient _client;
    private readonly IUserProfileService _userProfileService;

    public MoviesRepository(HttpClient client, IUserProfileService userProfileService)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _userProfileService = userProfileService;
    }

    public async Task<IEnumerable<SearchMovieDto>> GetPopularMovies()
    {
        var settings = await _userProfileService.GetUserSettings();
        
        var param = new Dictionary<string, string>();
        if (settings.Region is not null) param.Add("region", settings.Region);
        if (settings.Language is not null) param.Add("language", settings.Language);

        var url = QueryHelpers.AddQueryString("/api/movies", param);
        using var response = await _client.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            return Enumerable.Empty<SearchMovieDto>();
        }
        
        var dtos = await HttpClientHelper.ReadFromJsonSafe<IEnumerable<SearchMovieDto>>(response);
        return dtos ?? Enumerable.Empty<SearchMovieDto>();
    }
}