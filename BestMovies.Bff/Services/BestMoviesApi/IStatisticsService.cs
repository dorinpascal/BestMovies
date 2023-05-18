using System.Threading.Tasks;
using BestMovies.Shared.Dtos.Movies;

namespace BestMovies.Bff.Services.BestMoviesApi;

public interface IStatisticsService
{
    Task<MovieStatsDto> GetMovieStats(int movieId);
}