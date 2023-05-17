using System;

namespace BestMovies.Api.Persistence.Entity;

public class User
{
    public string Id { get; }
    public string Email { get; }

    public User(string id, string email)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        Email = email ?? throw new ArgumentNullException(nameof(email));
    }

#pragma warning disable CS8618
    public User()
    {
        // Needed by EF Core
    }
#pragma warning restore CS8618
}