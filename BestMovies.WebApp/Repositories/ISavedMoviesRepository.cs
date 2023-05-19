using BestMovies.Shared.Dtos.Movies;

namespace BestMovies.WebApp.Repositories;

public interface ISavedMoviesRepository
{
    Task SaveMovie(SavedMovieDto movieDto);
    Task<SavedMovieDto?> GetSavedMovie(int movieId);
    Task<IEnumerable<SearchMovieDto>> GetSavedMovies();
    Task RemoveMovie(int movieId);
}