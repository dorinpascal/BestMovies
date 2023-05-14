using BestMovies.Shared.CustomExceptions;
using BestMovies.Shared.Dtos.Review;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using BestMovies.Bff.Services.BestMoviesApi;
using BestMovies.Bff.BestMoviesAPIClient;

namespace BestMovies.Bff.Services.BestMoviesApi.Impl;

public class ReviewService : IReviewService
{
    private readonly IBestMoviesApiClient _client;

    public ReviewService(IBestMoviesApiClient client)
    {
        _client = client;
    }
    public async Task AddReview(string userId, ReviewDto review)
    {
        await _client.AddReview(userId, review);
    }
}
