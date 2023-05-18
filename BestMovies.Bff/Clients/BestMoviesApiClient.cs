using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BestMovies.Bff.Extensions;
using BestMovies.Shared.Dtos.Review;
using BestMovies.Shared.Dtos.User;

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
            await responseMessage.ThrowBasedOnStatusCode();
        }
    }

    public async Task SaveUser(CreateUserDto user)
    {
        var userJson = JsonSerializer.Serialize(user);
        var userStringContent = new StringContent(
            userJson,
            Encoding.UTF8,
            "application/json"
        );
        var responseMessage = await _client.PostAsync("users", userStringContent);
        if (!responseMessage.IsSuccessStatusCode)
        {
            await responseMessage.ThrowBasedOnStatusCode();
        }
    }

    public async Task<UserDto> GetUser(string userId)
    {
        var responseMessage = await _client.GetAsync($"users/{userId}");
        if (!responseMessage.IsSuccessStatusCode)
        {
            await responseMessage.ThrowBasedOnStatusCode();
        }
        
        var content = await responseMessage.ReadContentSafe();
        return JsonSerializer.Deserialize<UserDto>(content)!;
    }

    public void Dispose() => _client.Dispose();

    public async Task<IEnumerable<ReviewDto>> GetReviewList(int movieId)
    {
        var responseMessage = await _client.GetAsync($"movies/{movieId:int}/reviews");
        if (!responseMessage.IsSuccessStatusCode)
        {
            await responseMessage.ThrowBasedOnStatusCode();
        }
        var content = await responseMessage.ReadContentSafe();
        return JsonSerializer.Deserialize<List<ReviewDto>>(content)!;
    }
}
