using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BestMovies.Bff.Extensions;
using BestMovies.Shared.CustomExceptions;
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
    
     public async Task<IEnumerable<ReviewDto>> GetReviewsForMovie(int movieId)
    {
        var responseMessage = await _client.GetAsync($"movies/{movieId}/reviews");
        if (!responseMessage.IsSuccessStatusCode)
        {
            await responseMessage.ThrowBasedOnStatusCode();
        }
        var content = await responseMessage.ReadContentSafe();
        
        return JsonSerializer.Deserialize<IList<ReviewDto>>(content, _jsonSerializerOptions) ?? Enumerable.Empty<ReviewDto>();
    }


    public async Task<ReviewDto> GetUserReviewForMovie(int movieId, string userId)
    {
        var responseMessage = await _client.GetAsync($"movies/{movieId}/review?userId={userId}");
        if (!responseMessage.IsSuccessStatusCode)
        {

            await responseMessage.ThrowBasedOnStatusCode();
        }
        var content = await responseMessage.ReadContentSafe();
        return JsonSerializer.Deserialize<ReviewDto>(content, _jsonSerializerOptions) ?? throw new NotFoundException("User review not found.");
    }
    
    public Task DeleteReview(int movieId, string userId)
    {
        return _client.DeleteAsync($"movies/{movieId}/reviews?userId={userId}");
    }
}
