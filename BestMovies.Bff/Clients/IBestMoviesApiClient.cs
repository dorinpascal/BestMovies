using System.Threading.Tasks;
using BestMovies.Shared.Dtos.Movies;
using BestMovies.Shared.Dtos.Review;
using BestMovies.Shared.Dtos.User;

namespace BestMovies.Bff.Clients;

public interface IBestMoviesApiClient
{
    Task AddReview(string userId, CreateReviewDto review);
    Task SaveUser(CreateUserDto user);
    Task<UserDto> GetUser(string userId);
    Task SaveMovie(string userId, SavedMovieDto savedMovieDto);
}
