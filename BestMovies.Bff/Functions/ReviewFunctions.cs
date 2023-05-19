using System;
using System.IO;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using BestMovies.Bff.Authorization;
using BestMovies.Bff.Helpers;
using BestMovies.Bff.Services.BestMoviesApi;
using BestMovies.Shared.CustomExceptions;
using BestMovies.Shared.Dtos.Review;
using BestMovies.Shared.Dtos.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace BestMovies.Bff.Functions;

public class ReviewFunctions
{
    private const string Tag = "Review";

    private readonly IReviewService _reviewService;

    public ReviewFunctions(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    [FunctionName(nameof(AddReview))]
    [OpenApiOperation(operationId: nameof(AddReview), tags: new[] {Tag})]
    [OpenApiRequestBody("application/json", typeof(CreateReviewDto))]
    [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int), Description = "The movie id.")]
    [OpenApiParameter(name: "x-ms-client-principal", In = ParameterLocation.Header, Required = true, Type = typeof(string), Description = "base64 of ClientPrincipal")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.OK, Description = "Successfully added the review")]
    public async Task<IActionResult> AddReview(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "movies/{id:int}/reviews")]
        HttpRequest req, int id, ILogger log)
    {
        try
        {
            var claims = req.RetrieveClaimsPrincipal();
            var userId = claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (claims.Identity is null || !claims.Identity.IsAuthenticated || string.IsNullOrEmpty(userId))
            {
                return ActionResultHelpers.UnauthorizedResult();
            }

            var review =
                JsonConvert.DeserializeObject<CreateReviewDto>(await new StreamReader(req.Body).ReadToEndAsync());
            if (review is null)
            {
                return ActionResultHelpers.BadRequestResult("Invalid parameters.");
            }

            if (id != review.MovieId)
            {
                return ActionResultHelpers.BadRequestResult("The movie id from path does not match the one from body");
            }

            var user = new CreateUserDto(
                Id: userId,
                Email: claims.Identity!.Name!
            );

            await _reviewService.AddReview(user, review);

            return new OkResult();
        }
        catch (DuplicateException ex)
        {
            return ActionResultHelpers.Conflict(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return ActionResultHelpers.BadRequestResult(ex.Message);
        }
        catch (Exception ex)
        {
            log.LogError(ex, "Error occured while adding a review");
            return ActionResultHelpers.ServerErrorResult();
        }
    }

    [FunctionName(nameof(GetReviewsForMovie))]
    [OpenApiOperation(operationId: nameof(GetReviewsForMovie), tags: new[] {Tag})]
    [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int), Description = "The movie id.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ReviewDto), Description = "Returns all reviews for the given movie.")]
    public async Task<IActionResult> GetReviewsForMovie(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "movies/{id:int}/reviews")] HttpRequest req, int id, ILogger log)
    {
        try
        {
            if (id <= 0)
            {
                return ActionResultHelpers.BadRequestResult("Invalid value for the id. The value must be greater than 0");
            }

            var reviews = await _reviewService.GetReviewsForMovie(id);
            return new OkObjectResult(reviews);
        }
        catch (ArgumentException ex)
        {
            return ActionResultHelpers.BadRequestResult(ex.Message);
        }
        catch (Exception ex)
        {
            log.LogError(ex, "Error occured while retrieving all reviews for the movie with id {MovieId}", id);
            return ActionResultHelpers.ServerErrorResult();
        }
    }


    [FunctionName(nameof(GetUserReviewForMovie))]
    [OpenApiOperation(operationId: nameof(GetUserReviewForMovie), tags: new[] { Tag })]
    [OpenApiParameter(name: "userId", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The user id.")]
    [OpenApiParameter(name: "movieId", In = ParameterLocation.Path, Required = true, Type = typeof(int), Description = "The movie id.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ReviewDto), Description = "Returns the user review for a movie. ")]
    public async Task<IActionResult> GetUserReviewForMovie(
       [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "movies/{movieId}/reviews/users/{userId}")] HttpRequest req, int movieId, string userId, ILogger log)
    {
        try
        {
            if (movieId <= 0 || string.IsNullOrWhiteSpace(userId))
            {
                return ActionResultHelpers.BadRequestResult("Invalid value for the id. The value must be greater than 0");
            }

            var review = await _reviewService.GetUserReviewForMovie(movieId,userId);
            return new OkObjectResult(review);
        }
        catch(NotFoundException ex)
        {
            return ActionResultHelpers.NotFoundResult(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return ActionResultHelpers.BadRequestResult(ex.Message);
        }
        catch (Exception ex)
        {
            log.LogError(ex, "Error occured while retrieving all reviews for the movie with id {movieId} and user with id {userId}", movieId,userId);
            return ActionResultHelpers.ServerErrorResult();
        }
    }
}