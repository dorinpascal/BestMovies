# BestMovies Inc.


### BestMovies.WebApp & BestMovies.Bff

Both projects are run as a Static Web App:
 * **BestMovies.WebApp** - the static content
 * **BestMovies.Bff** - the azure functions

###### Run Static Web App locally
**Note:** Install `swa` using `npm install -g @azure/static-web-apps-cli`
```
swa start http://localhost:5124 --run "dotnet watch run --project ./BestMovies.WebApp/BestMovies.WebApp.csproj" --api-devserver-url http://localhost:7071
```

**Note:** Make sure you use Node v20.0.0+ and you have `local.settings.json` file

Add `BestMoviesApi.BaseUrl` to your `local.settings.json` file

### BestMovies.Api
The project is an azure functions

To run locally the project:
1. Create a database in your local Sql Server 
2. Get the connection string to it.
3. Create a new `local.settings.json` file similar to one in the **BestMovies.Bff** project. 
4. Add the `DbConnectionString` to your `local.settings.json` file

