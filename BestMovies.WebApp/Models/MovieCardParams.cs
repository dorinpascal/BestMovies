using BestMovies.Shared.Dtos.Movies;

namespace BestMovies.WebApp.Models;

public record MovieCardParams(bool LoadLazy, SearchMovieDto? Movie, int Width, int Height);