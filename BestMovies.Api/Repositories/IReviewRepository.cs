using System.Threading.Tasks;

namespace BestMovies.Api.Repositories;

public interface IReviewRepository
{
    Task CreateReview(string userId, int rating, string? comment);
}
