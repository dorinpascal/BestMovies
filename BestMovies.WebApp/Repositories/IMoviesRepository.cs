using BestMovies.Shared.Dtos.Movies;

namespace BestMovies.WebApp.Repositories;

public interface IMoviesRepository
{
    Task<IEnumerable<SearchMovieDto>> GetPopularMovies();
}