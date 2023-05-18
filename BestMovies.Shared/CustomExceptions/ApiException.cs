namespace BestMovies.Shared.CustomExceptions;

public class ApiException : Exception
{
    public ApiException(string message, int statusCode): base(message) {}
}