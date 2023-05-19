using System.Collections.Generic;
using System.Threading.Tasks;
using BestMovies.Api.Persistence.Entity;
using BestMovies.Shared.Dtos.Review;

namespace BestMovies.Api.Repositories;

public interface IReviewRepository
{
    Task CreateReview(int movieId, string userId, int rating, string? comment);

    Task<IEnumerable<Review>> GetReviewsForMovie(int movieId);
    Task<Review> GetUserReviewForMovie(int movieId, string userId);
}
