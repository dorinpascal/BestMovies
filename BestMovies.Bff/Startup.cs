
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TMDbLib.Client;

[assembly: FunctionsStartup(typeof(BestMovies.Bff.Startup))]
namespace BestMovies.Bff;


public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddHttpClient();

        builder.Services.AddScoped<TMDbClient>(c => new TMDbClient(""));
    }
}