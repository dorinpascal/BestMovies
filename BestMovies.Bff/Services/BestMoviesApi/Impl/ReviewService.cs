using System;
using System.Collections.Generic;
using System.Linq;
using BestMovies.Shared.Dtos.Review;
using System.Threading.Tasks;
using BestMovies.Bff.Clients;
using BestMovies.Shared.Dtos.User;
using FluentValidation;
using BestMovies.Shared.Dtos.Movies;

namespace BestMovies.Bff.Services.BestMoviesApi.Impl;

public class ReviewService : IReviewService
{
    private readonly IUserService _userService;
    private readonly ISavedMovieService _savedMovieService;
    private readonly IBestMoviesApiClient _client;
    private readonly IValidator<CreateReviewDto> _validator;

    public ReviewService(IUserService userService, ISavedMovieService savedMovieService, IBestMoviesApiClient client, IValidator<CreateReviewDto> validator)
    {
        _userService = userService;
        _savedMovieService = savedMovieService;
        _client = client;
        _validator = validator;
    }
    
    public async Task AddReview(UserDto user, CreateReviewDto review)
    {
        var validationResult = await _validator.ValidateAsync(review);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("; ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new ArgumentException(errors);
        }

        await _userService.GetUserOrCreate(user);
        
        var savedMovie = await _savedMovieService.GetSavedMovieOrDefault(review.MovieId, user.Id);
        if (savedMovie is null)
        {
            await _savedMovieService.SaveMovie(new SavedMovieDto(review.MovieId, true), user);
        }
        else
        {
            var watchedMovie = savedMovie with { IsWatched = true };
            await _savedMovieService.UpdateMovie(watchedMovie, user.Id);
        }
        
        await _client.AddReview(user.Id, review);
    }
    
    

    public async Task<IEnumerable<ReviewDto>> GetReviewsForMovie(int movieId, bool onlyReviewsWithComments = false)
    {
        var reviews = await _client.GetReviewsForMovie(movieId);

        return onlyReviewsWithComments 
            ? reviews.Where(r => !string.IsNullOrEmpty(r.Comment)) 
            : reviews;
    }

    public async Task<ReviewDto> GetUserReviewForMovie(int movieId, string userId)
    {
       
        return await _client.GetUserReviewForMovie(movieId, userId);
       
    }

    public Task DeleteReview(int movieId, string userId)
    {
        return _client.DeleteReview(movieId, userId);
    }
}
