using BestMovies.Shared.Dtos.Movies;

namespace BestMovies.WebApp.Repositories;

public interface ISavedMoviesRepository
{
    Task SaveMovie(SavedMovieDto movieDto);
    Task<IEnumerable<SavedMovieDto>> GetSavedMovies();
    Task RemoveMovie(int movieId);

    Task<bool> IsMovieSaved(int movieId);
}