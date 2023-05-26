using BestMovies.Bff.Clients;
using BestMovies.Bff.Services.Tmdb.Impl;
using BestMovies.Shared.CustomExceptions;
using BestMovies.Shared.Dtos.Movies;
using BestMovies.Shared.Dtos.Review;
using BestMovies.Shared.Dtos.User;
using Bogus;
using Bogus.Distributions.Gaussian;
using FakeDataGenerator;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using TMDbLib.Client;


const string appSettingsFileName = "appsettings.json";
var config =  new ConfigurationBuilder()
    .AddJsonFile(appSettingsFileName)
    .AddUserSecrets<Program>()
    .Build();

Log.Logger = new LoggerConfiguration()
    //.MinimumLevel.Verbose() -> for all the logs
    .MinimumLevel.Information()
    .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Verbose)
    .CreateLogger();

Log.Information("--- Fake Data Generator ---");

var baseUrl = config["AppSettings:BestMovies.Api.BaseUrl"] 
              ?? throw new ArgumentException("Please set the BestMovies.Api.BaseUrl");

Log.Information("Using AppSettings from {AppSettingsFileName} BaseUrl={BaseUrl}", 
    appSettingsFileName, baseUrl);

try
{
    Log.Information("Initializing services...");
    using var client = new HttpClient
    {
        BaseAddress = new Uri(baseUrl)
    };
    client.DefaultRequestHeaders.Add("x-functions-key", config["AppSettings:MasterKey"]);

    var movieService = new MovieService(new TMDbWrapperService(new TMDbClient(config["AppSettings:TmdbApiKey"])));
    var bestMoviesApiClient = new BestMoviesApiClient(client);

    var userFaker = new Faker<UserDto>()
        .CustomInstantiator(f => new UserDto(
            Id: f.Random.Uuid().ToString(),
            Email: f.Person.Email)
        );
    
    var faker = new Faker();

    var popularMovies = await movieService.GetPopularMovies();
    Log.Information("Retrieved {MovieCount} popular movies from TMDB", popularMovies.Count());

    const int numberOfUsers = 100;
    Log.Information("Starting the generation of fake data for {NumberOfUsers} users...", numberOfUsers);

    foreach (var user in userFaker.GenerateLazy(numberOfUsers))
    {
        Log.Information("Generating reviews for {User}", user);

        try
        {
            await bestMoviesApiClient.SaveUser(user);
        }
        catch (DuplicateException)
        {
            Log.Error("User with id {UserId} already present. Spiking to the next user", user.Id);
            continue;
        }
        
        foreach (var movie in popularMovies ?? Enumerable.Empty<SearchMovieDto>())
        {
            if (Math.Abs(faker.Random.GaussianDecimal(0, 1)) > 2)
            {
                // if outside of 2∂ -> skip the movie
                Log.Warning("Skipping movie {MovieTitle}", movie.Title);
                continue;
            }
            
            if (Math.Abs(faker.Random.GaussianDecimal(0, 1)) > 1)
            {
                // if outside of 1∂ -> set movie as want to watch
                Log.Debug("Setting movie {MovieTitle} as want to watch", movie.Title);
                await bestMoviesApiClient.SaveMovie(user.Id, new SavedMovieDto(movie.Id, false));
                continue;
            }

            var movieDetails = await movieService.GetMovieDetails(movie.Id);
            var avgVote = Math.Round(movieDetails.VoteAverage / 2, 0, MidpointRounding.AwayFromZero);
            
            var rating = faker.Random.GaussianInt((int)avgVote, 1);

            var review = new CreateReviewDto(
                    MovieId: movie.Id,
                    Rating: rating,
                    Comment: ReviewCommentGenerator.Generate(faker, rating)
                );                
            
            Log.Verbose("Add review {Review} for movie {MovieTitle}", review, movie.Title);
            await bestMoviesApiClient.SaveMovie(user.Id, new SavedMovieDto(movie.Id, true));
            await bestMoviesApiClient.AddReview(user.Id, review);
        }
    }

    Log.Information("Successfully generated fake data");
}
catch (Exception e)
{
    Log.Information(e, "Error during generation");
}

