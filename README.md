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

### BestMovies.Api
