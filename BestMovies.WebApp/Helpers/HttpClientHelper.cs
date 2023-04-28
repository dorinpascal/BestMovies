using System.Net.Http.Json;

namespace BestMovies.WebApp.Helpers;

public static class HttpClientHelper
{
    public static async Task<string> ReadContentSafe(HttpResponseMessage response)
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
    
    public static async Task<T?> ReadFromJsonSafe<T>(HttpResponseMessage response)
    {
        try
        {
            return await response.Content.ReadFromJsonAsync<T>();
        }
        catch (Exception)
        {
            return default;
        }
    }
}