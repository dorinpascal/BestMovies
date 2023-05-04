using Microsoft.AspNetCore.Mvc;

namespace BestMovies.Bff.Helpers;

public static class ExceptionHelpers
{

    public static ContentResult CreateContentResult(int statusCode, string content)
    {
        if (statusCode == 500) content = "The server is temporarily unable to handle the request, please try again later!";

        return new ContentResult
        {
            StatusCode = statusCode,
            Content = content,
            ContentType = "text/plain"
        };
    }
}
