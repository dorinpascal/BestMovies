using System.Collections.Generic;
using System.Threading.Tasks;

namespace BestMovies.Bff.Services;

public interface IGenreService
{
    public Task<IEnumerable<string>> GetGenreNames();
}