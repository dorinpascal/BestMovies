namespace BestMovies.Bff.Test.Helpers;

public abstract class MockLogger<T> : ILogger<T>
{
#pragma warning disable CS8769 
    void ILogger.Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) =>
        Log(logLevel, formatter(state, exception));
#pragma warning restore CS8769 

    public abstract void Log(LogLevel logLevel, string message);

    public virtual bool IsEnabled(LogLevel logLevel) => true;

    public abstract IDisposable BeginScope<TState>(TState state);
}
