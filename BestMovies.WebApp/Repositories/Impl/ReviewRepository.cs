using System.Text;
using System.Text.Json;
using BestMovies.Shared.CustomExceptions;
using BestMovies.Shared.Dtos.Review;
using BestMovies.WebApp.Helpers;

namespace BestMovies.WebApp.Repositories.Impl;

public class ReviewRepository : IReviewRepository
{
    private readonly HttpClient _client;
    private const string BaseUri = "/api/movies";

    public ReviewRepository(HttpClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }
    
    public async Task AddReview(CreateReviewDto review)
    {
        var json = JsonSerializer.Serialize(review);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _client.PostAsync($"{BaseUri}/{review.MovieId}/reviews", content);
        
        if (!response.IsSuccessStatusCode)
        {
            throw new ApiException(await HttpClientHelper.ReadContentSafe(response), (int)response.StatusCode);
        }
    }

    public async Task<IEnumerable<ReviewDto>> GetReviewsForMovie(int movieId, bool onlyReviewsWithComments = false)
    {
        using var response = await _client.GetAsync($"{BaseUri}/{movieId}/reviews?onlyReviewsWithComments={onlyReviewsWithComments}");
        
        if (!response.IsSuccessStatusCode)
        {
            return Enumerable.Empty<ReviewDto>();
        }

        var genres = await HttpClientHelper.ReadFromJsonSafe<IEnumerable<ReviewDto>>(response);
        return genres ?? Enumerable.Empty<ReviewDto>();
    }
    
    public async Task<ReviewDto?> GetMovieReview(int movieId)
    {
        using var response = await _client.GetAsync($"{BaseUri}/{movieId}/review");

        if (response.IsSuccessStatusCode)
        {
            return await HttpClientHelper.ReadFromJsonSafe<ReviewDto>(response);
        }

        return null;
    }
}