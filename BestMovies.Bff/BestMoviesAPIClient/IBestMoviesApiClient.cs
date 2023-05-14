using BestMovies.Shared.Dtos.Review;
using System.Threading.Tasks;

namespace BestMovies.Bff.BestMoviesAPIClient;

public interface IBestMoviesApiClient
{
    Task AddReview(string userId, ReviewDto review);
}
