using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;

namespace BestMovies.Bff.Clients;

public partial class BestMoviesApiClient : IBestMoviesApiClient, IDisposable
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

    public BestMoviesApiClient(HttpClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public void Dispose() => _client.Dispose();
}