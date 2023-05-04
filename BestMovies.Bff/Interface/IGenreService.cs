using System.Collections.Generic;
using System.Threading.Tasks;

namespace BestMovies.Bff.Interface;

public interface IGenreService
{
    public Task<IEnumerable<string>> GetGenreNames();
}