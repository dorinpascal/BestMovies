using System.Collections.Generic;
using BestMovies.Shared.Dtos.Review;
using System.Threading.Tasks;
using BestMovies.Shared.Dtos.User;

namespace BestMovies.Bff.Services.BestMoviesApi;

public interface IReviewService
{
    Task AddReview(UserDto user, CreateReviewDto review);
    Task<IEnumerable<ReviewDto>> GetReviewsForMovie(int movieId, bool onlyReviewsWithComments = false);
    Task<ReviewDto> GetUserReviewForMovie(int movieId, string userId);
    Task DeleteReview(int movieId, string userId);
}
