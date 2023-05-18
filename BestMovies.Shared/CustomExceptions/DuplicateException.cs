namespace BestMovies.Shared.CustomExceptions;

public class DuplicateException: Exception
{
    public DuplicateException(string message): base(message) {}
}