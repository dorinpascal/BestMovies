using System;
using System.Linq;
using System.Threading.Tasks;
using BestMovies.Bff.Clients;
using BestMovies.Shared.CustomExceptions;
using BestMovies.Shared.Dtos.User;
using FluentValidation;

namespace BestMovies.Bff.Services.BestMoviesApi.Impl;

public class UserService : IUserService
{
    private readonly IBestMoviesApiClient _client;
    private readonly IValidator<CreateUserDto> _validator;

    public UserService(IBestMoviesApiClient client, IValidator<CreateUserDto> validator)
    {
        _client = client;
        _validator = validator;
    }

    public async Task SaveUser(CreateUserDto user)
    {
        var validationResult = await _validator.ValidateAsync(user);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("; ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new ArgumentException(errors);
        }
        
        await _client.SaveUser(user);
    }

    public async Task<UserDto?> GetUserOrDefault(string userId)
    {
        try
        {
            return await _client.GetUser(userId);
        }
        catch (NotFoundException)
        {
            return null;
        }
    }
}