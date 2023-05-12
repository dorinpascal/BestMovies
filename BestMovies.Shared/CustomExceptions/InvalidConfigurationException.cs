namespace BestMovies.Shared.CustomExceptions;

public class InvalidConfigurationException : Exception
{
    public InvalidConfigurationException() {}
    public InvalidConfigurationException(string message) : base(message){ }
}
