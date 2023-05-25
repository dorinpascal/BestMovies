using System.Threading.Tasks;
using BestMovies.Api.Persistence;
using BestMovies.Api.Persistence.Entity;
using BestMovies.Shared.CustomExceptions;
using Microsoft.EntityFrameworkCore;

namespace BestMovies.Api.Repositories.Impl;

public class UserRepository : IUserRepository
{
    private readonly BestMoviesDbContext _dbContext;

    public UserRepository(BestMoviesDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task SaveUser(string userId, string email)
    {
        var user = new User(userId, email);

        var existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (existingUser is not null)
        {
            throw new DuplicateException($"A user with id '{userId}' is already saved");
        }

        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<User> GetUser(string identifier)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == identifier || u.Email == identifier) 
               ?? throw new NotFoundException($"Cannot find user with identifier {identifier}");
    }
}