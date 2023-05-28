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
    Task SaveUser(UserDto user);
    Task<UserDto> GetUser(string identifier);
    Task SaveMovie(string userId, SavedMovieDto savedMovie);
    Task UpdateMovie(string userDtoId, SavedMovieDto savedMovieDto);
    Task DeleteMovie(string userDtoId, int savedMovieId);
    Task<IEnumerable<SavedMovieDto>> GetSavedMoviesForUser(string userId, bool? isWatched);
    Task<SavedMovieDto> GetSavedMovie(string userId, int movieId);
    Task<ReviewDto> GetUserReviewForMovie(int movieId, string userId);
    Task<MovieStatsDto> GetMovieStats(int movieId);
    Task<decimal> GetAverageRatingOfMovies(IEnumerable<int> movieIds);
    Task DeleteReview(int movieId, string userId);
}
