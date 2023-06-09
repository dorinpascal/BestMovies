using BestMovies.Shared.Dtos.Movies;
using BestMovies.WebApp.Helpers;

namespace BestMovies.WebApp.Repositories.Impl;

public class StatisticsRepository : IStatisticsRepository
{
    private readonly HttpClient _client;
    private const string BaseUri = "/api/movies";

    public StatisticsRepository(HttpClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }
    
    public async Task<MovieStatsDto?> GetMovieStatistics(int movieId)
    {
        using var response = await _client.GetAsync($"{BaseUri}/{movieId}/stats");

        if (response.IsSuccessStatusCode)
        {
            return await HttpClientHelper.ReadFromJsonSafe<MovieStatsDto>(response);
        }

        return null;
    }
}