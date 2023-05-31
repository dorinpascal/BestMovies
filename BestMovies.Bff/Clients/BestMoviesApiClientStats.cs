using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BestMovies.Bff.Extensions;
using BestMovies.Shared.Dtos.Movies;

namespace BestMovies.Bff.Clients;

public partial class BestMoviesApiClient
{
    public async Task<MovieStatsDto> GetMovieStats(int movieId)
    {
        var responseMessage = await _client.GetAsync($"movies/{movieId}/stats");
        if (!responseMessage.IsSuccessStatusCode)
        {
            await responseMessage.ThrowBasedOnStatusCode();
        }
        var content = await responseMessage.ReadContentSafe();
        
        return JsonSerializer.Deserialize<MovieStatsDto>(content, _jsonSerializerOptions)!;
    }
    
    public async Task<decimal> GetAverageRatingOfMovies(IEnumerable<int> movieIds)
    {
        var url = $"movies/stats?movieIds={string.Join(",", movieIds)}";
        
        var responseMessage = await _client.GetAsync(url);
        if (!responseMessage.IsSuccessStatusCode)
        {
            await responseMessage.ThrowBasedOnStatusCode();
        }
        
        var content = await responseMessage.ReadContentSafe();
        
        return JsonSerializer.Deserialize<decimal>(content, _jsonSerializerOptions)!;
    }

    public async Task<IEnumerable<int>> GetTopRatedMovies()
    {
        var responseMessage = await _client.GetAsync($"movies/topRated");
        
        if (!responseMessage.IsSuccessStatusCode)
        {
            await responseMessage.ThrowBasedOnStatusCode();
        }
        
        var content = await responseMessage.ReadContentSafe();
        
        return JsonSerializer.Deserialize<IEnumerable<int>>(content, _jsonSerializerOptions)!;
    }
}