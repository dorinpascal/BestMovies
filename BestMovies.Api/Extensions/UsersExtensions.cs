using BestMovies.Api.Persistence.Entity;
using BestMovies.Shared.Dtos.User;

namespace BestMovies.Api.Extensions;

public static class UsersExtensions
{
    public static SimpleUserDto ToSimpleDto(this User user) => new(user.Email);
    public static UserDto ToDto(this User user) => new(user.Id, user.Email);
}