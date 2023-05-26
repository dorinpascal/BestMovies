using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;


const string appSettingsFileName = "appsettings.json";
var config =  new ConfigurationBuilder()
    .AddJsonFile(appSettingsFileName)
    .AddUserSecrets<Program>()
    .Build();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Debug)
    .CreateLogger();

Log.Information("--- Fake Data Generator ---");

var baseUrl = config["AppSettings:BestMovies.Api.BaseUrl"] 
              ?? throw new ArgumentException("Please set the BestMovies.Api.BaseUrl");

Log.Information("Using AppSettings from {AppSettingsFileName} BaseUrl={BaseUrl}", 
    appSettingsFileName, baseUrl);

try
{
    Log.Information("Initializing services...");
    using var client = new HttpClient
    {
        BaseAddress = new Uri(baseUrl)
    };
    client.DefaultRequestHeaders.Add("x-functions-key", config["AppSettings:MasterKey"]);
    
    Log.Information("Staring the generating fake data...");
    var response = await client.GetAsync("users/causion01@gmail.com");

    var content = await response.Content.ReadAsStringAsync();

    Console.WriteLine(response.StatusCode);
    Console.WriteLine(content);
    
    if (response.IsSuccessStatusCode)
    {

    }
    
    Log.Information("Successfully generated fake data");
}
catch (Exception e)
{
    Log.Information(e, "Error during generation");
}

