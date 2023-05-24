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
    Task<IEnumerable<SearchMovieDto>> GetSavedMoviesForUser(string userId, bool? isWatched = null);
    Task<SavedMovieDto?> GetSavedMovieOrDefault(int movieId, string userId);
}