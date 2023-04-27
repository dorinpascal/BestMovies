using System;
using TMDbLib.Client;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(BestMovies.Bff.Startup))]
namespace BestMovies.Bff;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddScoped<TMDbClient>(c => new TMDbClient(Environment.GetEnvironmentVariable("TMDB_API_KEY")));
        builder.Services.AddHttpClient();
    }
}