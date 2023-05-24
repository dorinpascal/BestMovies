using System.Threading.Tasks;
using BestMovies.Shared.Dtos.Movies;
using System.Collections.Generic;
using BestMovies.Shared.Dtos.Review;
using BestMovies.Shared.Dtos.User;

namespace BestMovies.Bff.Clients;

public interface IBestMoviesApiClient
{
    Task AddReview(string userId, CreateReviewDto review);
    Task<IEnumerable<ReviewDto>> GetReviewsForMovie(int movieId);
    Task SaveUser(CreateUserDto user);
    Task<UserDto> GetUser(string userId);
    Task SaveMovie(string userId, SavedMovieDto savedMovie);
    Task UpdateMovie(string userDtoId, SavedMovieDto savedMovieDto);
    Task DeleteMovie(string userDtoId, int savedMovieDto);
    Task<IEnumerable<SavedMovieDto>> GetSavedMoviesForUser(string userDtoId, bool? isWatched);
    Task<SavedMovieDto> GetSavedMovie(string userId, int movieId);
    Task<ReviewDto> GetUserReviewForMovie(int movieId, string userId);
    Task<MovieStatsDto> GetMovieStats(int movieId);
}
