using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BestMovies.Bff.Clients;
using BestMovies.Shared.Dtos.Movies;
using BestMovies.Shared.Dtos.User;
using FluentValidation;

namespace BestMovies.Bff.Services.BestMoviesApi.Impl;

public class SavedMovieService : ISavedMovieService
{
    private readonly IUserService _userService;
    private readonly IBestMoviesApiClient _client;
    private readonly IValidator<SavedMovieDto> _validator;

    public SavedMovieService(IValidator<SavedMovieDto> validator, IBestMoviesApiClient client, IUserService userService)
    {
        _validator = validator;
        _client = client;
        _userService = userService;
    }

    public async Task SaveMovie(SavedMovieDto savedMovieDto, CreateUserDto userDto)
    {
        await ValidateSavedMovie(savedMovieDto);
        
        await _userService.GetUserOrCreate(userDto);

        await _client.SaveMovie(userDto.Id, savedMovieDto);

    }

    public async Task UpdateMovie(SavedMovieDto savedMovieDto, CreateUserDto userDto)
    {
        await ValidateSavedMovie(savedMovieDto);

        await _client.UpdateMovie(userDto.Id, savedMovieDto);
    }

    public async Task DeleteMovie(int movieId, CreateUserDto userDto)
    {
        await _client.DeleteMovie(userDto.Id, movieId);
    }

    public async Task<IEnumerable<SavedMovieDto>> GetSavedMoviesForUser(CreateUserDto userDto, bool onlyUnwatched)
    {
        return await _client.GetSavedMoviesForUser(userDto.Id, onlyUnwatched);
    }

    private async Task ValidateSavedMovie(SavedMovieDto savedMovieDto)
    {
        var validationResult = await _validator.ValidateAsync(savedMovieDto);

        if (!validationResult.IsValid)
        {
            var errors = string.Join("; ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new ArgumentException(errors);
        }
    }
}