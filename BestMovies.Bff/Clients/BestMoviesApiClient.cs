using System;
using System.Net.Http;

namespace BestMovies.Bff.Clients;

public partial class BestMoviesApiClient : IBestMoviesApiClient, IDisposable
{
    private readonly HttpClient _client;

    public BestMoviesApiClient(HttpClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public void Dispose() => _client.Dispose();
}