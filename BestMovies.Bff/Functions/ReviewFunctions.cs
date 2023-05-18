using System;
using System.IO;
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
    [OpenApiOperation(operationId: nameof(AddReview), tags: new[] { Tag })]
    [OpenApiRequestBody("application/json", typeof(CreateReviewDto))]
    [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int), Description = "The movie id.")]
    [OpenApiParameter(name: "x-ms-client-principal", In = ParameterLocation.Header, Required = true, Type = typeof(string), Description = "base64 of ClientPrincipal")]
    public async Task<IActionResult> AddReview(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "movies/{id:int}/reviews")] HttpRequest req, int id, ILogger log)
    {
        try
        {
            var claims = req.RetrieveClaimsPrincipal();
            var userId = claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (claims.Identity is null || !claims.Identity.IsAuthenticated || string.IsNullOrEmpty(userId))
            {
                return ActionResultHelpers.UnauthorizedResult();
            }

            var review = JsonConvert.DeserializeObject<CreateReviewDto>(await new StreamReader(req.Body).ReadToEndAsync());
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

    [FunctionName(nameof(GetReviews))]
    [OpenApiOperation(operationId: nameof(GetReviews), tags: new[] { Tag })]
    [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int), Description = "The movie id.")]
      public async Task<IActionResult> GetReviews(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "movies/{id:int}/reviews")] HttpRequest req, int movieId, ILogger log)
      {
        try
        {
            if (movieId <= 0)
            {
                return ActionResultHelpers.BadRequestResult("Invalid value for the id. The value must be greater than 0");
            }
            var reviews = await _reviewService.GetMovieReviews(movieId);
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

}

