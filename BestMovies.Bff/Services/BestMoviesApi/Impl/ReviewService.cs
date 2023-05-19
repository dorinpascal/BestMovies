using System;
using System.Collections.Generic;
using System.Linq;
using BestMovies.Shared.Dtos.Review;
using System.Threading.Tasks;
using BestMovies.Bff.Clients;
using BestMovies.Shared.Dtos.User;
using FluentValidation;
using BestMovies.Shared.CustomExceptions;

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
    
    public async Task AddReview(CreateUserDto user, CreateReviewDto review)
    {
        var validationResult = await _validator.ValidateAsync(review);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("; ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new ArgumentException(errors);
        }

        await _userService.GetUserOrCreate(user);
        
        
        //ToDo set the movie as watched, if not there - add it
        
        await _client.AddReview(user.Id, review);
    }
    
    

    public async Task<IEnumerable<ReviewDto>> GetReviewsForMovie(int movieId)
    {
        return await _client.GetReviewsForMovie(movieId);
    }

    public async Task<ReviewDto> GetUserReviewForMovie(int movieId, string userId)
    {
       
        return await _client.GetUserReviewForMovie(movieId, userId);
       
    }
}
