using System;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
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
            var message = await responseMessage.Content.ReadAsStringAsync();

            switch (responseMessage.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    throw new ArgumentException(message);
                case HttpStatusCode.Unauthorized:
                case HttpStatusCode.Forbidden:
                    throw new AuthenticationException(message);
                case HttpStatusCode.InternalServerError:
                default:
                    throw new Exception(message);
            }
            
        }
    }

    public void Dispose() => _client.Dispose();
}
