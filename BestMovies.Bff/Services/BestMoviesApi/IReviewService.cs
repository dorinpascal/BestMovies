using BestMovies.Shared.Dtos.Review;
using System.Threading.Tasks;

namespace BestMovies.Bff.Services.BestMoviesApi;

public interface IReviewService
{
    Task AddReview(string userId, ReviewDto review);
}
