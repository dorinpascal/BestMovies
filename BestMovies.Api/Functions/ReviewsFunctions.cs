using BestMovies.Api.Helpers;
using BestMovies.Api.Services;
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
using System.Net;
using System.Threading.Tasks;

namespace BestMovies.Api.Functions;

public class AddReview
{
    private readonly ILogger<AddReview> _logger;
    private readonly IReviewService _reviewService;
    public AddReview(ILogger<AddReview> log, IReviewService reviewService)
    {
        _logger = log;
        _reviewService = reviewService;
    }

    [FunctionName(nameof(AddReview))]
    [OpenApiOperation(operationId: nameof(AddReview), tags: new[] { "Review" })]
    [OpenApiRequestBody("application/json", typeof(ReviewDto))]
    [OpenApiParameter(name: "userId", In = ParameterLocation.Path, Required = true, Type = typeof(int), Description = "The user id.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(IEnumerable<ReviewDto>), Description = "Add a review.")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "user/{userId}/review")] HttpRequest req,int userId)
    { 
        try
        {
            var review = JsonConvert.DeserializeObject<ReviewDto>(await new StreamReader(req.Body).ReadToEndAsync());
            if (review is null) return ActionResultHelpers.BadRequestResult("Invalid parameters.");
            await _reviewService.CreateReview(userId, review);
            return new OkObjectResult("The review was added with success.");
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Error occured while retrieving actor details");
            return ActionResultHelpers.ServerErrorResult();
        }
       
    }
}

