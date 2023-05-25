using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using BestMovies.Api.Extensions;
using BestMovies.Api.Helpers;
using BestMovies.Api.Repositories;
using BestMovies.Shared.CustomExceptions;
using BestMovies.Shared.Dtos.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace BestMovies.Api.Functions;

public class UsersFunctions
{
    private const string Tag = "User";
    
    private readonly IUserRepository _userRepository;
     
    public UsersFunctions(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [FunctionName(nameof(SaveUser))]
    [OpenApiOperation(operationId: nameof(SaveUser), tags: new[] { Tag })]
    [OpenApiRequestBody("application/json", typeof(UserDto))]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.OK, Description = "Successfully saved the user")]
    public async Task<IActionResult> SaveUser(
        [HttpTrigger(AuthorizationLevel.Admin, "post", Route = "users")] HttpRequest req, ILogger log)
    {
        try
        {
            var userDto = JsonConvert.DeserializeObject<UserDto>(await new StreamReader(req.Body).ReadToEndAsync());
            if (userDto is null)
            {
                return ActionResultHelpers.BadRequestResult("Invalid parameters.");
            }

            await _userRepository.SaveUser(userDto.Id, userDto.Email);
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
        catch(Exception ex)
        {
            log.LogError(ex, "Error occured while saving a user");
            return ActionResultHelpers.ServerErrorResult();
        }
    }
    
    [FunctionName(nameof(GetUser))]
    [OpenApiOperation(operationId: nameof(GetUser), tags: new[] { Tag })]
    [OpenApiParameter(name: "identifier", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The user id or email.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(UserDto), Description = "Retrieve user based on the id")]
    public async Task<IActionResult> GetUser(
        [HttpTrigger(AuthorizationLevel.Admin, "get", Route = "users/{identifier}")] HttpRequest req, string identifier, ILogger log)
    { 
        if (string.IsNullOrWhiteSpace(identifier))
        {
            return ActionResultHelpers.BadRequestResult("Invalid value for the user identifier.");
        }
        
        try
        {
            var user = await _userRepository.GetUser(identifier);

            return new OkObjectResult(user.ToDto());
        }
        catch (NotFoundException ex)
        {
            return ActionResultHelpers.NotFoundResult(ex.Message);
        }
        catch(Exception ex)
        {
            log.LogError(ex, "Error occured while retrieving a user");
            return ActionResultHelpers.ServerErrorResult();
        }
    }
}