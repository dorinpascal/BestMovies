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

    public User()
    {
        
    }
}