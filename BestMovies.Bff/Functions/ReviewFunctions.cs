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
    private const string Tag = "Review";
    
    private readonly IReviewService _reviewService;

    public ReviewFunctions(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    [FunctionName(nameof(AddReview))]
    [OpenApiOperation(operationId: nameof(AddReview), tags: new[] { Tag })]
    [OpenApiRequestBody("application/json", typeof(CreateReviewDto))]
    [OpenApiParameter(name: "userId", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The user id.")]
    public async Task<IActionResult> AddReview(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "users/{userId}/reviews")] HttpRequest req, string userId, ILogger log)
    {
        var review = JsonConvert.DeserializeObject<CreateReviewDto>(await new StreamReader(req.Body).ReadToEndAsync());
        if (review is null || string.IsNullOrEmpty(userId))
        {
            return ActionResultHelpers.BadRequestResult("Invalid parameters.");
        }

        //Todo: return Unauthorized if not logged in
        try
        {
            await _reviewService.AddReview(userId, review);
            return new OkResult();
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

