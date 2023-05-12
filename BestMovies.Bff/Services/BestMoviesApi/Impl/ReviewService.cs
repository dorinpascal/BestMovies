using BestMovies.Shared.CustomExceptions;
using BestMovies.Shared.Dtos.Review;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;

namespace BestMovies.Bff.Services.BestMoviesApi.Impl;

public class ReviewService : IReviewService
{
    private readonly HttpClient _client;

    public ReviewService()
    {
        _client = new HttpClient();
    }
    public async Task AddReview(string uri, string userId, ReviewDto review)
    {
        if (string.IsNullOrEmpty(uri)) throw new InvalidConfigurationException("BestMoviesApiUrl is not available in Appsettings");

        var reviewJson = JsonSerializer.Serialize(review, new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        var reviewStringContent = new StringContent(
            reviewJson,
            Encoding.UTF8,
            "application/json"
        );
        var responseMessage = await _client.PostAsync($"{uri}/user/{userId}/reviews", reviewStringContent);
        if (!responseMessage.IsSuccessStatusCode)
        {
            var jsonObject = await JsonDocument.ParseAsync(await responseMessage.Content.ReadAsStreamAsync());
            throw new Exception(jsonObject.RootElement.GetProperty("message").GetString());
        }
    }
}
