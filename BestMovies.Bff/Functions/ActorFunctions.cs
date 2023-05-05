using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using BestMovies.Bff.Helpers;
using BestMovies.Bff.Interface;
using BestMovies.Shared.CustomExceptions;
using BestMovies.Shared.Dtos.Actor;
using BestMovies.Shared.Dtos.Movies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace BestMovies.Bff.Functions;

public class ActorFunctions
{
    private const string Tag = "Actor";

    private readonly IActorService _actorService;

    public ActorFunctions(IActorService actorService)
    {
        _actorService = actorService;
    }
    
    [FunctionName(nameof(GetActorDetails))]
    [OpenApiOperation(operationId: nameof(GetActorDetails), tags: new[] { Tag })]
    [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int), Description = "The actor id.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ActorDetailsDto), Description = "Return details about the actor")]
    public async Task<IActionResult> GetActorDetails(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "actors/{id:int}")] HttpRequest req, int id, ILogger log)
    {
        if (id <= 0)
        {
            return ActionResultHelpers.BadRequestResult("Invalid value for the id. The value must be greater than 0");
        }
        
        try
        {
            var actorDetails = await _actorService.GetActorDetails(id);
            return new OkObjectResult(actorDetails);
        }
        catch (NotFoundException ex)
        {
            return ActionResultHelpers.NotFoundResult(ex.Message);
        }
        catch (Exception ex)
        {
            log.LogError(ex, "Error occured while retrieving actor details");
            return ActionResultHelpers.ServerErrorResult();
        }
    }
}