using System.Collections.Generic;

namespace BestMovies.Shared
{
    public class MoviesDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Image { get; set; }
        public List<int> Genres { get; set; } = new List<int>();
    }
}