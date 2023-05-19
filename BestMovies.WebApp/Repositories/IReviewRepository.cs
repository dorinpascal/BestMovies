using BestMovies.Shared.Dtos.Review;

namespace BestMovies.WebApp.Repositories;

public interface IReviewRepository
{
    Task AddReview(CreateReviewDto review);
    Task<IEnumerable<ReviewDto>> GetReviewsForMovie(int movieId, bool onlyReviewsWithComments = false);
    Task<ReviewDto?> GetMovieReview(int movieId);
}