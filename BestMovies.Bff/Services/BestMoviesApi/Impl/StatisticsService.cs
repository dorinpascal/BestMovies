using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BestMovies.Bff.Clients;
using BestMovies.Bff.Extensions;
using BestMovies.Bff.Services.Tmdb;
using BestMovies.Shared.Dtos.Movies;

namespace BestMovies.Bff.Services.BestMoviesApi.Impl;

public class StatisticsService : IStatisticsService
{
    private readonly IBestMoviesApiClient _client;
    private readonly ITMDbWrapperService _tmDbService;

    public StatisticsService(IBestMoviesApiClient client, ITMDbWrapperService tmDbService)
    {
        _client = client;
        _tmDbService = tmDbService;
    }
    
    public async Task<MovieStatsDto> GetMovieStats(int movieId)
    {
        return await _client.GetMovieStats(movieId);
    }

    public async Task<IEnumerable<SearchMovieDto>> GetTopRatedMovies()
    {
        var topRatedMovieIds = await _client.GetTopRatedMovies();
        var tasks = topRatedMovieIds.Select(id => _tmDbService.GetMovieAsync(id));
        var popularMovies = await Task.WhenAll(tasks);

        return popularMovies.Where(m => m is not null).Select(m => m!.ToSearchDto());
    }
}