using BestMovies.Shared.Dtos.Movies;

namespace BestMovies.WebApp.Repositories;

public interface IStatisticsRepository
{
    Task<MovieStatsDto?> GetMovieStatistics(int movieId);
    Task<IEnumerable<SearchMovieDto>> GetTopRatedMovies();
}