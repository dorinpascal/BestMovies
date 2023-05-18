using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BestMovies.Bff.Extensions;
using BestMovies.Shared.Dtos.Review;

namespace BestMovies.Bff.Clients;

public partial class BestMoviesApiClient
{
    public async Task AddReview(string userId, CreateReviewDto review)
    {
        var reviewJson = JsonSerializer.Serialize(review);
        var reviewStringContent = new StringContent(
            reviewJson,
            Encoding.UTF8,
            "application/json"
        );
        var responseMessage = await _client.PostAsync($"users/{userId}/reviews", reviewStringContent);
        if (!responseMessage.IsSuccessStatusCode)
        {
            await responseMessage.ThrowBasedOnStatusCode();
        }
    }
    
}
