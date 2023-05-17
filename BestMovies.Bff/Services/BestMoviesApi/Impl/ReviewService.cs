using BestMovies.Shared.Dtos.Review;
using System.Threading.Tasks;
using BestMovies.Bff.Clients;

namespace BestMovies.Bff.Services.BestMoviesApi.Impl;

public class ReviewService : IReviewService
{
    private readonly IBestMoviesApiClient _client;

    public ReviewService(IBestMoviesApiClient client)
    {
        _client = client;
    }
    public async Task AddReview(string userId, CreateReviewDto review)
    {
        // ToDo : add validation for email
        await _client.AddReview(userId, review);
    }
}
