using System.Collections.Generic;
using System.Threading.Tasks;
using BestMovies.Shared.Dtos.Movies;
using BestMovies.Shared.Dtos.User;

namespace BestMovies.Bff.Services.BestMoviesApi;

public interface ISavedMovieService
{
    Task SaveMovie(SavedMovieDto savedMovieDto, CreateUserDto userDto);
    Task UpdateMovie(SavedMovieDto savedMovieDto, CreateUserDto userDto);
    Task DeleteMovie(int movieId, CreateUserDto userDto);
    Task<IEnumerable<SavedMovieDto>> GetSavedMoviesForUser(CreateUserDto userDto, bool onlyUnwatched);
}