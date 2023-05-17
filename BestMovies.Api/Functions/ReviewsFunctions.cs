using BestMovies.Api.Helpers;
using BestMovies.Shared.Dtos.Review;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BestMovies.Api.Extensions;
using BestMovies.Api.Repositories;

namespace BestMovies.Api.Functions;

public class ReviewFunctions
{
    private const string Tag = "Review";
    
    private readonly IReviewRepository _reviewRepository;
    public ReviewFunctions(IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }

    [FunctionName(nameof(AddReview))]
    [OpenApiOperation(operationId: nameof(AddReview), tags: new[] { Tag })]
    [OpenApiRequestBody("application/json", typeof(CreateReviewDto))]
    [OpenApiParameter(name: "userId", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The user id.")]
    public async Task<IActionResult> AddReview(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "users/{userId}/reviews")] HttpRequest req, string userId, ILogger log)
    { 
        try
        {
            var reviewDto = JsonConvert.DeserializeObject<ReviewDto>(await new StreamReader(req.Body).ReadToEndAsync());
            if (reviewDto is null)
            {
                return ActionResultHelpers.BadRequestResult("Invalid parameters.");
            }
            
            await _reviewRepository.CreateReview(reviewDto.MovieId, userId, reviewDto.Rating, reviewDto.Comment);
            return new OkResult();
        }
        catch (ArgumentException ex)
        {
            return ActionResultHelpers.BadRequestResult(ex.Message);
        }
        catch(Exception ex)
        {
            log.LogError(ex, "Error occured while adding a review");
            return ActionResultHelpers.ServerErrorResult();
        }
    }
    
    [FunctionName(nameof(GetReviewsForMovie))]
    [OpenApiOperation(operationId: nameof(GetReviewsForMovie), tags: new[] { Tag })]
    [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int), Description = "The movie id.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(IEnumerable<ReviewDto>), Description = "Retrieve reviews for a specific movie")]
    public async Task<IActionResult> GetReviewsForMovie(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "movies/{id:int}/reviews")] HttpRequest req, int id, ILogger log)
    { 
        if (id <= 0)
        {
            return ActionResultHelpers.BadRequestResult("Invalid value for the id. The value must be greater than 0");
        }
        
        try
        {
            var reviews = await _reviewRepository.GetReviewsForMovie(id);
            var dtos = reviews.Select(review => review.ToDto());
            
            return new OkObjectResult(dtos);
        }
        catch(Exception ex)
        {
            log.LogError(ex, "Error occured while retrieving reviews for movie with id {MovieId}", id);
            return ActionResultHelpers.ServerErrorResult();
        }
    }
}

