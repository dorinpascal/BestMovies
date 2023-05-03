namespace BestMovies.Shared.CustomExceptions;

public class InvalidParameterException : Exception
{
    public InvalidParameterException(string message)
        : base(message) {}

    public InvalidParameterException(){}

}
