using System;
using System.Linq;
using BestMovies.Shared.Dtos.Review;
using System.Threading.Tasks;
using BestMovies.Bff.Clients;
using BestMovies.Shared.Dtos.User;
using FluentValidation;

namespace BestMovies.Bff.Services.BestMoviesApi.Impl;

public class ReviewService : IReviewService
{
    private readonly IUserService _userService;
    private readonly IBestMoviesApiClient _client;
    private readonly IValidator<CreateReviewDto> _validator;

    public ReviewService(IUserService userService, IBestMoviesApiClient client, IValidator<CreateReviewDto> validator)
    {
        _userService = userService;
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
        
        var existingUser = await  _userService.GetUserOrDefault(user.Id);
        if (existingUser is null)
        {
            await _userService.SaveUser(user);
        }
        
        await _client.AddReview(user.Id, review);
    }

    public async Task<ReviewListDto> GetMovieReviews(int movieId)
    {
        var reviews = await _client.GetReviewList(movieId);
    }
}
