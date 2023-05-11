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
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace BestMovies.Api.Functions;

public class ReviewsFunctions
{
    private readonly ILogger<ReviewsFunctions> _logger;
    private readonly IReviewService _reviewService;
    public ReviewsFunctions(ILogger<ReviewsFunctions> log, IReviewService reviewService)
    {
        _logger = log;
        _reviewService = reviewService;
    }

    [FunctionName(nameof(ReviewsFunctions))]
    [OpenApiOperation(operationId: nameof(ReviewsFunctions), tags: new[] { "Review" })]
    [OpenApiRequestBody("application/json", typeof(ReviewDto))]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(IEnumerable<ReviewDto>), Description = "Add a review to the database.")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "review")] HttpRequest req)
    {

        var review = JsonConvert.DeserializeObject<ReviewDto>(await new StreamReader(req.Body).ReadToEndAsync());

       await _reviewService.CreateReview(review);

        return new OkObjectResult(null);
    }
}

