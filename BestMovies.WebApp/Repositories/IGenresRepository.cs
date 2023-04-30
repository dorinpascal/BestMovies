namespace BestMovies.WebApp.Repositories;

public interface IGenresRepository
{

    Task<IEnumerable<string>> GetAllGenres();
}