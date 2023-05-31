using BestMovies.Shared.Dtos.Movies;

namespace BestMovies.WebApp.Repositories;

public interface IMoviesRepository
{
    Task<IEnumerable<SearchMovieDto>> GetPopularMovies(string? genre = null);
    Task<IEnumerable<SearchMovieDto>> SearchMovie(string movieTitle);
    Task<MovieDetailsDto?> GetMovieDetails(int id);
    Task<IEnumerable<SearchMovieDto>> GetTopRatedMovies();
}