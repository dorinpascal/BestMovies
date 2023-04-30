using BestMovies.WebApp.Helpers;

namespace BestMovies.WebApp.Repositories;

public class GenresRepository : IGenresRepository
{
    private readonly HttpClient _client;
    private const string BaseUri = "/api/genres";

    public GenresRepository(HttpClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async Task<IEnumerable<string>> GetAllGenres()
    {
        using var response = await _client.GetAsync(BaseUri);
        
        if (!response.IsSuccessStatusCode)
        {
            return Enumerable.Empty<string>();
        }

        var genres = await HttpClientHelper.ReadFromJsonSafe<IEnumerable<string>>(response);
        return genres ?? Enumerable.Empty<string>();
    }
}