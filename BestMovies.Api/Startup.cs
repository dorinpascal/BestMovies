using System;
using System.IO;
using System.Reflection;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Migr8;

[assembly: FunctionsStartup(typeof(BestMovies.Api.Startup))]
namespace BestMovies.Api;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        var dbConnectionString = Environment.GetEnvironmentVariable("DbConnectionString") 
                                 ?? throw new ArgumentException("Please make sure you have DbConnectionString as an environmental variable");
        builder.Services.AddDbContext<BestMoviesDbContext>(options =>
        {
            options.UseSqlServer(dbConnectionString);
        });
        
        MigrateDatabase(dbConnectionString);
    }

    private static void MigrateDatabase(string connectionString)
    {
        try
        {
            var migrations = Migr8.Migrations.FromAssemblyOf<Startup>();

            var options = new Options(
                migrationTableName: "__BestMoviesDatabaseMigrations"
            );
            
            Database.Migrate(
                connectionString: connectionString,
                migrations: migrations,
                options: options
            );
        }
        catch (Exception e)
        {
            throw new Exception($"Could not migrate database with connection string '{connectionString}'", e);
        }
    }
}
