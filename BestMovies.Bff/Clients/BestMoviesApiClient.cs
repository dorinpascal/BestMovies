using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BestMovies.Shared.Dtos.Review;

namespace BestMovies.Bff.Clients;

public class BestMoviesApiClient : IBestMoviesApiClient, IDisposable
{
    private readonly HttpClient _client;
    public BestMoviesApiClient(HttpClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));   
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
        var responseMessage = await _client.PostAsync($"/user/{userId}/reviews", reviewStringContent);
        if (!responseMessage.IsSuccessStatusCode)
        {
            var jsonObject = await JsonDocument.ParseAsync(await responseMessage.Content.ReadAsStreamAsync());
            throw new Exception(jsonObject.RootElement.GetProperty("message").GetString());
        }
    }

    public void Dispose() => _client.Dispose();
}
