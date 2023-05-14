using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BestMovies.WebApp;
using BestMovies.WebApp.Repositories;
using BestMovies.WebApp.Services;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(_ => new HttpClient {BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)});

// Services
builder.Services.AddScoped<IUserProfileService, StaticUserProfileService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Repositories
builder.Services.AddTransient<IMoviesRepository, MoviesRepository>();
builder.Services.AddTransient<IGenresRepository, GenresRepository>();
builder.Services.AddTransient<IActorsRepository, ActorsRepository>();

// Mud Blazor
builder.Services.AddMudServices();

await builder.Build().RunAsync();