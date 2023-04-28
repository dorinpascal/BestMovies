using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using TMDbLib.Client;

namespace BestMovies.Bff.Functions;

public class ImageFunctions
{
    private const string Tag = "Image";
    
    private readonly TMDbClient _tmDbClient;

    public ImageFunctions(TMDbClient tmDbClient)
    {
        _tmDbClient = tmDbClient;
    }
    
    [FunctionName(nameof(GetImageByPath))]
    [OpenApiOperation(operationId: nameof(GetImageByPath), tags: new[] { Tag })]
    [OpenApiParameter(name: "path", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The path to the poster.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "image/jpg", bodyType: typeof(byte[]), Description = "Returns popular movies in the region.")]
    public async Task<IActionResult> GetImageByPath(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "images/{path}")] HttpRequest req,
        string path,
        ILogger log)
    {
        Console.WriteLine(path);
        var config = await _tmDbClient.GetConfigAsync();
        Console.WriteLine(string.Join(", ",
         
            config.Images.PosterSizes));
        var imageBytes = await _tmDbClient.GetImageBytesAsync("w500", "/" + path);
        
        return new FileContentResult(imageBytes, "image/jpg");
    }
}