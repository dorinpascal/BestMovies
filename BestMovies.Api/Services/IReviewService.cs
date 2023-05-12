using BestMovies.Shared.Dtos.Review;
using System.Threading.Tasks;

namespace BestMovies.Api.Services;

public interface IReviewService
{
    Task CreateReview(string UserId, ReviewDto reviewDto);
}
