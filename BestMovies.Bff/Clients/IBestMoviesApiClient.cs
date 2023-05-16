using System.Threading.Tasks;
using BestMovies.Shared.Dtos.Review;

namespace BestMovies.Bff.Clients;

public interface IBestMoviesApiClient
{
    Task AddReview(string userId, ReviewDto review);
}
