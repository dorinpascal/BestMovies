using BestMovies.Shared.Dtos.Person;
using BestMovies.WebApp.Helpers;

namespace BestMovies.WebApp.Repositories.Impl;

public class ActorsRepository : IActorsRepository
{
    private readonly HttpClient _client;
    private const string BaseUri = "/api/actors";

    public ActorsRepository(HttpClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }
    
    public async Task<PersonDetailsDto?> GetActorDetails(int id)
    {
        using var response = await _client.GetAsync($"{BaseUri}/{id}");

        if (response.IsSuccessStatusCode)
        {
            return await HttpClientHelper.ReadFromJsonSafe<PersonDetailsDto>(response);
        }

        return null;
    }
}