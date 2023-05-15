using System.Collections.Generic;
using System.Threading.Tasks;

namespace BestMovies.Bff.Services.Tmdb;

public interface IGenreService
{
    public Task<IEnumerable<string>> GetGenreNames();
}