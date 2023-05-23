using System.Text.Json;
using System.Threading.Tasks;
using BestMovies.Bff.Extensions;
using BestMovies.Shared.Dtos.Movies;

namespace BestMovies.Bff.Clients;

public partial class BestMoviesApiClient
{
    public async Task<MovieStatsDto> GetMovieStats(int movieId)
    {
        var responseMessage = await _client.GetAsync($"movies/{movieId}/stats/");
        if (!responseMessage.IsSuccessStatusCode)
        {
            await responseMessage.ThrowBasedOnStatusCode();
        }
        var content = await responseMessage.ReadContentSafe();
        
        return JsonSerializer.Deserialize<MovieStatsDto>(content, _jsonSerializerOptions)!;
    }
}