using System;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Threading.Tasks;
using BestMovies.Shared.CustomExceptions;

namespace BestMovies.Bff.Extensions;

public static class HttpResponseMessageExtensions
{
    public static async Task ThrowBasedOnStatusCode(this HttpResponseMessage response)
    {
        var message = await response.ReadContentSafe();

        switch (response.StatusCode)
        {
            case HttpStatusCode.BadRequest:
                throw new ArgumentException(message);
            
            case HttpStatusCode.NotFound:
                throw new NotFoundException(message);
            
            case HttpStatusCode.Unauthorized:
            case HttpStatusCode.Forbidden:
                throw new AuthenticationException(message);
            
            case HttpStatusCode.Conflict:
                throw new DuplicateException(message);
            
            case HttpStatusCode.InternalServerError:
            default:
                throw new Exception(message);
        }
    } 
    
    public static async Task<string> ReadContentSafe(this HttpResponseMessage response)
    {
        try
        {
            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception exception)
        {
            return $"Error reading content from response: {exception}";
        }
    }
}