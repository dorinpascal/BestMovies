﻿using System;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using BestMovies.Bff.Authorization;
using BestMovies.Bff.Helpers;
using BestMovies.Bff.Services.BestMoviesApi;
using BestMovies.Shared.CustomExceptions;
using BestMovies.Shared.Dtos.Movies;
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
    [OpenApiParameter(name: "x-ms-client-principal", In = ParameterLocation.Header, Required = true,
        Type = typeof(string), Description = "base64 of ClientPrincipal")]
    public async Task<IActionResult> SaveMovie(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "savedMovies")]
        HttpRequest req, ILogger log)
    {
        try
        {
            if (AuthenticationHelpers.AuthenticateUser(req, out var user, out var actionResult)) return actionResult;

            var savedMovie =
                JsonConvert.DeserializeObject<SavedMovieDto>(await new StreamReader(req.Body).ReadToEndAsync());
            
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
    [OpenApiParameter(name: "x-ms-client-principal", In = ParameterLocation.Header, Required = true,
        Type = typeof(string), Description = "base64 of ClientPrincipal")]
    public async Task<IActionResult> UpdateSavedMovie(
        [HttpTrigger(AuthorizationLevel.Anonymous, "patch", Route = "savedMovies")]
        HttpRequest req, ILogger log)
    {
        try
        {
            if (AuthenticationHelpers.AuthenticateUser(req, out var user, out var actionResult)) return actionResult;

            var savedMovie =
                JsonConvert.DeserializeObject<SavedMovieDto>(await new StreamReader(req.Body).ReadToEndAsync());
            
            if (savedMovie is null)
            {
                return ActionResultHelpers.BadRequestResult("Invalid parameters.");
            }
            
            await _savedMovieService.UpdateMovie(savedMovie, user!);

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
    [OpenApiParameter(name: "x-ms-client-principal", In = ParameterLocation.Header, Required = true,
        Type = typeof(string), Description = "base64 of ClientPrincipal")]
    public async Task<IActionResult> DeleteSavedMovie(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "savedMovies/{movieId:int}")]
        HttpRequest req, int movieId, ILogger log)
    {
        try
        {
            if (AuthenticationHelpers.AuthenticateUser(req, out var user, out var actionResult)) return actionResult;
            
            await _savedMovieService.DeleteMovie(movieId, user!);

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
    [OpenApiParameter(name: "onlyUnwatched", In = ParameterLocation.Query, Required = true, Type = typeof(bool), Description = "Get only unwatched movies.")]
    [OpenApiParameter(name: "x-ms-client-principal", In = ParameterLocation.Header, Required = true,
        Type = typeof(string), Description = "base64 of ClientPrincipal")]
    public async Task<IActionResult> GetSavedMovies(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "savedMovies")]
        HttpRequest req, ILogger log)
    {
        try
        {
            if (AuthenticationHelpers.AuthenticateUser(req, out var user, out var actionResult)) return actionResult;

            if (!bool.TryParse(req.Query["onlyUnwatched"], out var onlyUnwatched))
            {
                return ActionResultHelpers.BadRequestResult(
                    "Only unwatched query parameter could not be converted to a boolean");
            }
            
            var savedMovies = await _savedMovieService.GetSavedMoviesForUser(user!, onlyUnwatched);

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
    
    //TODO implement this vertical slice
    // [FunctionName(nameof(GetSavedMovie))]
    // [OpenApiOperation(operationId: nameof(GetSavedMovie), tags: new[] {Tag})]
    // [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int), Description = "The movie id.")]
    // [OpenApiParameter(name: "x-ms-client-principal", In = ParameterLocation.Header, Required = true,
    //     Type = typeof(string), Description = "base64 of ClientPrincipal")]
    // public async Task<IActionResult> GetSavedMovie(
    //     [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "savedMovies/{movieId:int}")]
    //     HttpRequest req, int id, ILogger log)
    // {
    //     try
    //     {
    //         if (AuthenticationHelpers.AuthenticateUser(req, out var user, out var actionResult)) return actionResult;
    //         
    //         await _savedMovieService.GetSavedMovie(id, user!);
    //
    //         return new OkResult();
    //     }
    //    
    //     catch (ArgumentException ex)
    //     {
    //         return ActionResultHelpers.BadRequestResult(ex.Message);
    //     }
    //     
    //     catch (Exception ex)
    //     {
    //         log.LogError(ex, "Error occured while deleting the movie");
    //         return ActionResultHelpers.ServerErrorResult();
    //     }
    // }
}
