using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using BestMovies.Bff.Helpers;
using BestMovies.Bff.Services.BestMoviesApi;
using BestMovies.Shared.CustomExceptions;
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

public class SavedMovieFunctions
{
    private const string Tag = "Saved Movies";

    private readonly ISavedMovieService _savedMovieService;

    public SavedMovieFunctions(ISavedMovieService savedMovieService)
    {
        _savedMovieService = savedMovieService;
    }

    [FunctionName(nameof(SaveMovie))]
    [OpenApiOperation(operationId: nameof(SaveMovie), tags: new[] {Tag})]
    [OpenApiRequestBody("application/json", typeof(SavedMovieDto))]
    [OpenApiParameter(name: "x-ms-client-principal", In = ParameterLocation.Header, Required = true, Type = typeof(string), Description = "base64 of ClientPrincipal")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.OK, Description = "Successfully saved the movie")]
    public async Task<IActionResult> SaveMovie(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "savedMovies")] HttpRequest req, ILogger log)
    {
        try
        {
            if (!AuthenticationHelpers.AuthenticateUser(req, out var user))
            {
                return ActionResultHelpers.UnauthorizedResult();
            }

            var savedMovie = JsonConvert.DeserializeObject<SavedMovieDto>(await new StreamReader(req.Body).ReadToEndAsync());
            
            if (savedMovie is null)
            {
                return ActionResultHelpers.BadRequestResult("Invalid parameters.");
            }
            
            await _savedMovieService.SaveMovie(savedMovie, user!);

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
            log.LogError(ex, "Error occured while saving the movie");
            return ActionResultHelpers.ServerErrorResult();
        }
    }
    
    [FunctionName(nameof(UpdateSavedMovie))]
    [OpenApiOperation(operationId: nameof(UpdateSavedMovie), tags: new[] {Tag})]
    [OpenApiRequestBody("application/json", typeof(SavedMovieDto))]
    [OpenApiParameter(name: "x-ms-client-principal", In = ParameterLocation.Header, Required = true, Type = typeof(string), Description = "base64 of ClientPrincipal")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.OK, Description = "Successfully updated the saved movie")]
    public async Task<IActionResult> UpdateSavedMovie(
        [HttpTrigger(AuthorizationLevel.Anonymous, "patch", Route = "savedMovies")] HttpRequest req, ILogger log)
    {
        try
        {
            if (!AuthenticationHelpers.AuthenticateUser(req, out var user))
            {
                return ActionResultHelpers.UnauthorizedResult();
            }

            var savedMovie = JsonConvert.DeserializeObject<SavedMovieDto>(await new StreamReader(req.Body).ReadToEndAsync());
            
            if (savedMovie is null)
            {
                return ActionResultHelpers.BadRequestResult("Invalid parameters.");
            }
            
            await _savedMovieService.UpdateMovie(savedMovie, user!.Id);

            return new OkResult();
        }
        catch (ArgumentException ex)
        {
            return ActionResultHelpers.BadRequestResult(ex.Message);
        }
        catch (Exception ex)
        {
            log.LogError(ex, "Error occured while updating the movie");
            return ActionResultHelpers.ServerErrorResult();
        }
    }
    
    [FunctionName(nameof(DeleteSavedMovie))]
    [OpenApiOperation(operationId: nameof(DeleteSavedMovie), tags: new[] {Tag})]
    [OpenApiParameter(name: "movieId", In = ParameterLocation.Path, Required = true, Type = typeof(int), Description = "The movie id.")]
    [OpenApiParameter(name: "x-ms-client-principal", In = ParameterLocation.Header, Required = true, Type = typeof(string), Description = "base64 of ClientPrincipal")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.OK, Description = "Successfully deleted the saved movie")]
    public async Task<IActionResult> DeleteSavedMovie(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "savedMovies/{movieId:int}")] HttpRequest req, int movieId, ILogger log)
    {
        try
        {
            if (!AuthenticationHelpers.AuthenticateUser(req, out var user))
            {
                return ActionResultHelpers.UnauthorizedResult();
            }
            
            await _savedMovieService.DeleteMovie(movieId, user!.Id);

            return new OkResult();
        }
       
        catch (ArgumentException ex)
        {
            return ActionResultHelpers.BadRequestResult(ex.Message);
        }
        
        catch (Exception ex)
        {
            log.LogError(ex, "Error occured while deleting the movie");
            return ActionResultHelpers.ServerErrorResult();
        }
    }
    
    [FunctionName(nameof(GetSavedMovies))]
    [OpenApiOperation(operationId: nameof(GetSavedMovies), tags: new[] {Tag})]
    [OpenApiParameter(name: "isWatched", In = ParameterLocation.Query, Required = false, Type = typeof(bool), Description = "Return only watched/unwatched movies.")]
    [OpenApiParameter(name: "x-ms-client-principal", In = ParameterLocation.Header, Required = true, Type = typeof(string), Description = "base64 of ClientPrincipal")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(IEnumerable<SearchMovieDto>), Description = "Returns the saved movies as searchMovie dto.")]
    public async Task<IActionResult> GetSavedMovies(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "savedMovies")]
        HttpRequest req, ILogger log)
    {
        try
        {
            if (!AuthenticationHelpers.AuthenticateUser(req, out var user))
            {
                return ActionResultHelpers.UnauthorizedResult();
            }

            IEnumerable<SearchMovieDto> savedMovies;
            
            if (!bool.TryParse(req.Query["isWatched"], out var isWatched))
            {
                savedMovies = await _savedMovieService.GetSavedMoviesForUser(user!.Id);
            }
            else
            {
                savedMovies = await _savedMovieService.GetSavedMoviesForUser(user!.Id, isWatched);
            }
            
            return new OkObjectResult(savedMovies);
        }
       
        catch (ArgumentException ex)
        {
            return ActionResultHelpers.BadRequestResult(ex.Message);
        }
        
        catch (Exception ex)
        {
            log.LogError(ex, "Error occured while retrieving movies for user");
            return ActionResultHelpers.ServerErrorResult();
        }
    }
    
     [FunctionName(nameof(GetSavedMovie))]
     [OpenApiOperation(operationId: nameof(GetSavedMovie), tags: new[] {Tag})]
     [OpenApiParameter(name: "movieId", In = ParameterLocation.Path, Required = true, Type = typeof(int), Description = "The movie id.")]
     [OpenApiParameter(name: "x-ms-client-principal", In = ParameterLocation.Header, Required = true, Type = typeof(string), Description = "base64 of ClientPrincipal")]
     [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(SavedMovieDto), Description = "Returns the saved movie. ")]
     public async Task<IActionResult> GetSavedMovie(
         [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "savedMovies/{movieId:int}")]
         HttpRequest req, int movieId, ILogger log)
     {
         try
         {
             if (!AuthenticationHelpers.AuthenticateUser(req, out var user))
             {
                 return ActionResultHelpers.UnauthorizedResult();
             }

             var savedMovie = await _savedMovieService.GetSavedMovieOrDefault(movieId, user!.Id);
             if (savedMovie is null)
             {
                 return ActionResultHelpers.NotFoundResult($"Saved movie with id {movieId} not found for user {user!.Id}");
             }

             return new OkObjectResult(savedMovie);
         }
         catch (NotFoundException)
         {
             return ActionResultHelpers.NotFoundResult(
                 $"Saved movie with id {movieId} not found for user ");
         }
         catch (ArgumentException ex)
         {
             return ActionResultHelpers.BadRequestResult(ex.Message);
         }
         catch (Exception ex)
         {
             log.LogError(ex, "Error occured while retrieving the movie");
             return ActionResultHelpers.ServerErrorResult();
         }
     }
}

