using BestMovies.Shared.Dtos.Review;
using System.Threading.Tasks;

namespace BestMovies.Api.Repo;

public interface IReviewService
{
    Task CreateReview(string UserId, ReviewDto reviewDto);
}
