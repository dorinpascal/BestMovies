using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestMovies.Shared.Dtos.Movies
{
    public record SearchMovieDto(int Id, string Title, string? PosterPath, IEnumerable<string> Genres);
}
