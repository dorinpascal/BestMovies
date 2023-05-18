using System.Collections.Generic;
using BestMovies.Shared.Dtos.Review;
using System.Threading.Tasks;
using BestMovies.Shared.Dtos.User;

namespace BestMovies.Bff.Services.BestMoviesApi;

public interface IReviewService
{
    Task AddReview(CreateUserDto user, CreateReviewDto review);
    Task<IEnumerable<ReviewDto>> GetReviews(int movieId);
}
