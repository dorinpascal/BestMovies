using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BestMovies.Bff.Clients;
using BestMovies.Bff.Extensions;
using BestMovies.Bff.Services.Tmdb;
using BestMovies.Shared.CustomExceptions;
using BestMovies.Shared.Dtos.Movies;
using BestMovies.Shared.Dtos.User;
using FluentValidation;

namespace BestMovies.Bff.Services.BestMoviesApi.Impl;

public class SavedMovieService : ISavedMovieService
{
    private readonly IUserService _userService;
    private readonly IBestMoviesApiClient _client;
    private readonly ITMDbWrapperService _tmDbService;
    private readonly IValidator<SavedMovieDto> _validator;

    public SavedMovieService(IValidator<SavedMovieDto> validator, IBestMoviesApiClient client, ITMDbWrapperService tmDbService, IUserService userService)
    {
        _validator = validator;
        _client = client;
        _tmDbService = tmDbService;
        _userService = userService;
    }

    public async Task SaveMovie(SavedMovieDto savedMovieDto, UserDto userDto)
    {
        await ValidateSavedMovie(savedMovieDto);
        
        await _userService.GetUserOrCreate(userDto);

        await _client.SaveMovie(userDto.Id, savedMovieDto);
    }

    public async Task UpdateMovie(SavedMovieDto savedMovieDto, string userId)
    {
        await ValidateSavedMovie(savedMovieDto);

        await _client.UpdateMovie(userId, savedMovieDto);
    }

    public async Task DeleteMovie(int movieId, string userId)
    {
        var movieToDelete = await GetSavedMovieOrDefault(movieId, userId);
        if (movieToDelete is null)
        {
            return;
        }

        if (movieToDelete.IsWatched)
        {
            throw new ArgumentException("Cannot delete a watched movie");
        }

        await _client.DeleteMovie(userId, movieId);
    }

    public async Task<IEnumerable<SearchMovieDto>> GetSavedMoviesForUser(string userId, bool? isWatched)
    {
        var savedMovies = await _client.GetSavedMoviesForUser(userId, isWatched);

        var tasks = savedMovies
            .Select(savedMovie => _tmDbService.GetMovieAsync(savedMovie.MovieId));

        var movies = await Task.WhenAll(tasks);

        return movies.Where(m => m is not null).Select(m => m!.ToSearchDto());
    }

    public async Task<SavedMovieDto?> GetSavedMovieOrDefault(int movieId, string userId)
    {
        try
        {
            return await _client.GetSavedMovie(userId, movieId);
        }
        catch (NotFoundException)
        {
            return null;
        }
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