using System;
using BestMovies.Bff.Clients;
using TMDbLib.Client;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using BestMovies.Bff.Services.BestMoviesApi;
using BestMovies.Bff.Services.BestMoviesApi.Impl;
using BestMovies.Bff.Services.Tmdb;
using BestMovies.Bff.Services.Tmdb.Impl;
using BestMovies.Shared.Dtos.Review;
using BestMovies.Shared.Dtos.User;
using BestMovies.Shared.Validators;
using FluentValidation;

[assembly: FunctionsStartup(typeof(BestMovies.Bff.Startup))]
namespace BestMovies.Bff;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        var bestMoviesApiBaseUrl = Environment.GetEnvironmentVariable("BestMoviesApi.BaseUrl") ?? throw new ArgumentException("Please make sure you have BestMoviesApi.BaseUrl as an environmental variable");

        builder.Services.AddHttpClient<IBestMoviesApiClient, BestMoviesApiClient>(client => client.BaseAddress = new Uri(bestMoviesApiBaseUrl));
        builder.Services.AddScoped<TMDbClient>(c => new TMDbClient(Environment.GetEnvironmentVariable("TMDB_API_KEY")));
        
        // Services
        builder.Services.AddTransient<IMovieService, MovieService>();
        builder.Services.AddTransient<IGenreService, GenreService>();
        builder.Services.AddTransient<IActorService, ActorService>();
        builder.Services.AddTransient<IReviewService, ReviewService>();
        builder.Services.AddTransient<IUserService, UserService>();
        builder.Services.AddTransient<IStatisticsService, StatisticsService>();
        
        builder.Services.AddTransient<ITMDbWrapperService, TMDbWrapperService>();
        
        // Validators
        builder.Services.AddScoped<IValidator<CreateUserDto>, CreateUserDtoValidator>();
        builder.Services.AddScoped<IValidator<CreateReviewDto>, CreateReviewDtoValidator>();
        
    }
}