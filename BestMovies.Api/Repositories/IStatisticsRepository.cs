using System.Collections.Generic;
using System.Threading.Tasks;
using BestMovies.Shared.Dtos.Movies;

namespace BestMovies.Api.Repositories;

public interface IStatisticsRepository
{
    Task<MovieStatsDto> GetMovieStats(int movieId);
    Task<decimal> GetAverageRatingOfMovies(IEnumerable<int> movieIds);
    public Task<IEnumerable<int>> GetTopRatedMovies();
}