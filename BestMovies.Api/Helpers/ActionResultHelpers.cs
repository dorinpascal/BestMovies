using Microsoft.AspNetCore.Mvc;
namespace BestMovies.Api.Helpers;

public static class ActionResultHelpers
{
    public static IActionResult Conflict(string content) => Result(409, content);
    public static IActionResult BadRequestResult(string content) => Result(400, content);

    public static IActionResult NotFoundResult(string content) => Result(404, content);

    public static IActionResult ServerErrorResult() => Result(500, "The server is temporarily unable to handle the request, please try again later!");

    private static ContentResult Result(int statusCode, string message) => new()
    {
        StatusCode = statusCode,
        Content = message,
        ContentType = "text/plain"
    };
}
