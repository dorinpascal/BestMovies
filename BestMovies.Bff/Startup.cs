using System;
using TMDbLib.Client;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using BestMovies.Bff.Services.Tmdb.Impl;
using BestMovies.Bff.Services.Tmdb;
using BestMovies.Bff.Services.BestMoviesApi;
using BestMovies.Bff.Services.BestMoviesApi.Impl;

[assembly: FunctionsStartup(typeof(BestMovies.Bff.Startup))]
namespace BestMovies.Bff;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddHttpClient();
        builder.Services.AddScoped<TMDbClient>(c => new TMDbClient(Environment.GetEnvironmentVariable("TMDB_API_KEY")));
        
        builder.Services.AddTransient<IMovieService, MovieService>();
        builder.Services.AddTransient<IGenreService, GenreService>();
        builder.Services.AddTransient<IActorService, ActorService>();
        builder.Services.AddTransient<IReviewService, ReviewService>();
        builder.Services.AddTransient<ITMDbWrapperService, TMDbWrapperService>();
    }
}