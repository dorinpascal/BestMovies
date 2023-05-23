using System.Threading.Tasks;
using BestMovies.Bff.Clients;
using BestMovies.Shared.Dtos.Movies;

namespace BestMovies.Bff.Services.BestMoviesApi.Impl;

public class StatisticsService : IStatisticsService
{
    private readonly IBestMoviesApiClient _client;

    public StatisticsService(IBestMoviesApiClient client)
    {
        _client = client;
    }
    
    public async Task<MovieStatsDto> GetMovieStats(int movieId)
    {
        return await _client.GetMovieStats(movieId);
    }
}