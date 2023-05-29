using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using BestMovies.Shared.Dtos.Movies;

namespace BestMovies.Bff.Services.BestMoviesApi;

public interface IStatisticsService
{
    Task<MovieStatsDto> GetMovieStats(int movieId);
    Task<IEnumerable<SearchMovieDto>> GetTopRatedMovies(List<SearchMovieDto> popularMovies);
}