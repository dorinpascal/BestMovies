using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using BestMovies.Bff.Authorization;
using BestMovies.Bff.Helpers;
using BestMovies.Bff.Services.BestMoviesApi;
using BestMovies.Shared.CustomExceptions;
using BestMovies.Shared.Dtos.Review;
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
    private readonly IUserService _userService;

    public ReviewFunctions(IReviewService reviewService, IUserService userService)
    {
        _reviewService = reviewService;
        _userService = userService;
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
            if (!AuthenticationHelpers.AuthenticateUser(req, out var user)) return ActionResultHelpers.UnauthorizedResult();

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
            
            await _reviewService.AddReview(user!, review);

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
    [OpenApiOperation(operationId: nameof(GetReviewsForMovie), tags: new[] { Tag })]
    [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int), Description = "The movie id.")]
    [OpenApiParameter(name: "onlyReviewsWithComments", In = ParameterLocation.Path, Required = false, Type = typeof(bool), Description = "The movie id.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(IEnumerable<ReviewDto>), Description = "Returns all reviews for the given movie.")]
    public async Task<IActionResult> GetReviewsForMovie(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "movies/{id:int}/reviews")] HttpRequest req, int id, ILogger log)
    {
        try
        {
            if (id <= 0)
            {
                return ActionResultHelpers.BadRequestResult("Invalid value for the id. The value must be greater than 0");
            }

            if (!bool.TryParse(req.Query["onlyReviewsWithComments"], out var onlyReviewsWithComments))
            {
                onlyReviewsWithComments = false;
            }

            var reviews = await _reviewService.GetReviewsForMovie(id, onlyReviewsWithComments);
            
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
    [OpenApiParameter(name: "movieId", In = ParameterLocation.Path, Required = true, Type = typeof(int), Description = "The movie id.")]
    [OpenApiParameter(name: "userEmail", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The user's email address.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ReviewDto), Description = "Returns the user review for a movie. ")]
    public async Task<IActionResult> GetUserReviewForMovie(
       [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "users/{userEmail}/movies/{movieId}/review")] HttpRequest req, string userEmail, int movieId, ILogger log)
    {
        try
        {
            if (movieId <= 0)
            {
                return ActionResultHelpers.BadRequestResult("Invalid value for the id. The value must be greater than 0");
            }
            
            var user = await _userService.GetUserOrDefault(userEmail);
            if (user is null)
            {
                return ActionResultHelpers.NotFoundResult($"Cannot find user with email '{userEmail}'");
            }

            var review = await _reviewService.GetUserReviewForMovie(movieId, user!.Id);
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
            log.LogError(ex, "Error occured while retrieving the review for the movie with id {MovieId}", movieId);
            return ActionResultHelpers.ServerErrorResult();
        }
    }
    
    [FunctionName(nameof(GetReviewForMovie))]
    [OpenApiOperation(operationId: nameof(GetReviewForMovie), tags: new[] { Tag })]
    [OpenApiParameter(name: "x-ms-client-principal", In = ParameterLocation.Header, Required = true, Type = typeof(string), Description = "base64 of ClientPrincipal")]
    [OpenApiParameter(name: "movieId", In = ParameterLocation.Path, Required = true, Type = typeof(int), Description = "The movie id.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ReviewDto), Description = "Returns the user review for a movie. ")]
    public async Task<IActionResult> GetReviewForMovie(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "movies/{movieId}/review")] HttpRequest req, int movieId, ILogger log)
    {
        try
        {
            if (!AuthenticationHelpers.AuthenticateUser(req, out var user)) 
            {
                return ActionResultHelpers.UnauthorizedResult();
            }

            if (movieId <= 0)
            {
                return ActionResultHelpers.BadRequestResult("Invalid value for the id. The value must be greater than 0");
            }

            var review = await _reviewService.GetUserReviewForMovie(movieId, user!.Id);
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
            log.LogError(ex, "Error occured while retrieving the review for the movie with id {MovieId}", movieId);
            return ActionResultHelpers.ServerErrorResult();
        }
    }

    [FunctionName(nameof(DeleteReview))]
    [OpenApiOperation(operationId: nameof(DeleteReview), tags: new[] {Tag})]
    [OpenApiParameter(name: "x-ms-client-principal", In = ParameterLocation.Header, Required = true, Type = typeof(string), Description = "base64 of ClientPrincipal")]
    [OpenApiParameter(name: "movieId", In = ParameterLocation.Path, Required = true, Type = typeof(int), Description = "The movie id.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.OK, Description = "Successfully deleted the review")]
    public async Task<IActionResult> DeleteReview(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "movies/{movieId}/review")]
        HttpRequest req, int movieId, ILogger log)
    {
        try
        {

            if (!AuthenticationHelpers.AuthenticateUser(req, out var user)) 
            {
                return ActionResultHelpers.UnauthorizedResult();
            }

            if (movieId <= 0)
            {
                return ActionResultHelpers.BadRequestResult("Invalid value for the id. The value must be greater than 0");
            }

            Console.WriteLine(movieId);
            
            await _reviewService.DeleteReview(movieId, user!.Id);

            return new OkResult();
        }
        catch (ArgumentException ex)
        {
            return ActionResultHelpers.BadRequestResult(ex.Message);
        }
        catch (Exception ex)
        {
            log.LogError(ex, "Error occured while deleting the review for the movie with id {MovieId}", movieId);
            return ActionResultHelpers.ServerErrorResult();
        }
    }
    
}