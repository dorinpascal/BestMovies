using System.Linq;
using System.Threading.Tasks;
using BestMovies.Bff.Clients;
using BestMovies.Shared.Dtos.Movies;

namespace BestMovies.Bff.Services.BestMoviesApi.Impl;

public class StatisticsService : IStatisticsService
{
    private readonly IBestMoviesApiClient _client;

    public StatisticsService(IBestMoviesApiClient client)
    {
        _client = client;
    }
    
    public async Task<MovieStatsDto> GetMovieStats(int movieId)
    {
        // ToDo: Move this to db as a query for performance
        var reviews = await _client.GetReviewsForMovie(movieId);
        var reviewsList = reviews.ToArray();

        var averageRating = (decimal)reviewsList.Average(r => r.Rating);
        var reviewsCount = reviewsList.Count(r => !string.IsNullOrEmpty(r.Comment));

        return new MovieStatsDto(averageRating, reviewsCount, 0, 0);
    }
}