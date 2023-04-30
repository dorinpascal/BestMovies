using BestMovies.Shared.Dtos.Movies;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BestMovies.Bff.Interface;

public interface ITmdbApiWrapper
{
    Task<IEnumerable<SearchMovieDto>> SearchMovie(string movieTitle);
    Task<IEnumerable<SearchMovieDto>> GetPopularMovies(string? language = null, string? region = null);
    Task<byte[]> GetImageBytes(string size, int id);
}
