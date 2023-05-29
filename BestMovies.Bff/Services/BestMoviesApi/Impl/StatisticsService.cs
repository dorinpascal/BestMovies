using System.Collections.Generic;
using System.Linq;
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

    public async Task<IEnumerable<SearchMovieDto>> GetTopRatedMovies(List<SearchMovieDto> popularMovies)
    {
        var popularMovieIds = popularMovies.Select(m => m.Id).ToList();
        var topRatedMovieIds = await _client.GetTopRatedMovies(popularMovieIds);
   
        var movieDictionary = popularMovies.ToDictionary(m => m.Id);
        var sortedMovies = topRatedMovieIds.Select(id => movieDictionary[id]).ToList();
        
        return sortedMovies;
    }
}