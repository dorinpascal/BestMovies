using BestMovies.Shared.Dtos.Review;
using System.Threading.Tasks;

namespace BestMovies.Api.Services
{
    public interface IReviewService
    {
        Task CreateReview(int UserId, ReviewDto reviewDto);
    }
}
