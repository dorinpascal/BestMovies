using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BestMovies.Bff.Interface;
using TMDbLib.Client;

namespace BestMovies.Bff.Services;

public class GenreService : IGenreService
{
    private readonly TMDbClient _tmDbClient;

    public GenreService(TMDbClient tmDbClient)
    {
        _tmDbClient = tmDbClient;
    }

    public async Task<IEnumerable<string>> GetGenreNames()
    {
        var genres = await _tmDbClient.GetMovieGenresAsync();
        return genres.Select(g => g.Name);
    }
}