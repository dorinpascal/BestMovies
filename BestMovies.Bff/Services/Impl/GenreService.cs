using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BestMovies.Bff.Services.Impl;

public class GenreService : IGenreService
{
    private readonly ITMDbWrapperService _tmDbClient;

    public GenreService(ITMDbWrapperService tmDbClient)
    {
        _tmDbClient = tmDbClient;
    }

    public async Task<IEnumerable<string>> GetGenreNames()
    {
        var genres = await _tmDbClient.GetMovieGenresAsync();
        return genres.Select(g => g.Name);
    }
}