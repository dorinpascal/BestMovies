using System.Collections.Generic;
using System.Threading.Tasks;
using BestMovies.Shared.Dtos.Movies;
using BestMovies.Shared.Dtos.User;

namespace BestMovies.Bff.Services.BestMoviesApi;

public interface ISavedMovieService
{
    Task SaveMovie(SavedMovieDto savedMovieDto, CreateUserDto userDto);
    Task UpdateMovie(SavedMovieDto savedMovieDto, string userId);
    Task DeleteMovie(int movieId, string userId);
    Task<IEnumerable<SavedMovieDto>> GetSavedMoviesForUser(string userId, bool onlyUnwatched);
    Task<SavedMovieDto?> GetSavedMovie(int movieId, string userId);
}