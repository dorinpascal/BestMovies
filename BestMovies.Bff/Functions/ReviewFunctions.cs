using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using BestMovies.Bff.Helpers;
using BestMovies.Bff.Services.BestMoviesApi;
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
    private readonly ILogger<ReviewFunctions> _logger;
    private readonly IReviewService _reviewService;

    public ReviewFunctions(ILogger<ReviewFunctions> log, IReviewService reviewService)
    {
        _logger = log;
        _reviewService = reviewService;
    }

    [FunctionName(nameof(AddReview))]
    [OpenApiOperation(operationId: nameof(AddReview), tags: new[] { "Review" })]
    [OpenApiRequestBody("application/json", typeof(ReviewDto))]
    [OpenApiParameter(name: "userId", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The user id.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(IEnumerable<ReviewDto>), Description = "Add a review.")]
    public async Task<IActionResult> AddReview(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "user/{userId}/reviews")] HttpRequest req, string userId)
    {
        var review = JsonConvert.DeserializeObject<ReviewDto>(await new StreamReader(req.Body).ReadToEndAsync());
        if (review is null || string.IsNullOrEmpty(userId)) return ActionResultHelpers.BadRequestResult("Invalid parameters.");
        try
        {
            await _reviewService.AddReview(userId, review);
            _logger.LogInformation( "Review added with success.");
            return new OkObjectResult("Review added with success.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occured while adding a review");
            return ActionResultHelpers.ServerErrorResult();
        }
    }
}

