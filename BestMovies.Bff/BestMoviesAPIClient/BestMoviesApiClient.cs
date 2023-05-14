using BestMovies.Shared.Dtos.Review;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BestMovies.Bff.BestMoviesAPIClient;

public class BestMoviesApiClient:IBestMoviesApiClient, IDisposable
{
    private readonly HttpClient _client;
    private readonly string? _uri = "https://fn-bestmovies-api-prod.azurewebsites.net/api";
    public BestMoviesApiClient()
    {
        _client = new HttpClient();   
    }


    public async Task AddReview(string userId, ReviewDto review)
    {
        var reviewJson = JsonSerializer.Serialize(review, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        var reviewStringContent = new StringContent(
            reviewJson,
            Encoding.UTF8,
            "application/json"
        );
        var responseMessage = await _client.PostAsync($"{_uri}/user/{userId}/reviews", reviewStringContent);
        if (!responseMessage.IsSuccessStatusCode)
        {
            var jsonObject = await JsonDocument.ParseAsync(await responseMessage.Content.ReadAsStreamAsync());
            throw new Exception(jsonObject.RootElement.GetProperty("message").GetString());
        }
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}
