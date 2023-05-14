using BestMovies.Shared.Dtos.Review;
using System.Threading.Tasks;

namespace BestMovies.Api.Repo;

public interface IReviewRepository
{
    Task CreateReview(string userId, ReviewDto reviewDto);
}
