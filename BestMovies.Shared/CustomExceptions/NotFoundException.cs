namespace BestMovies.Shared.CustomExceptions;

public class NotFoundException:Exception
{
    public NotFoundException() { }
    public NotFoundException(string message): base(message) {}
}
