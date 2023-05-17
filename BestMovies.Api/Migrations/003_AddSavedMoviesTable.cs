namespace BestMovies.Api.Migrations;

using Migr8;


[Migration(3, "Create MovieList Table")]

public class _003_AddSavedMoviesTable : ISqlMigration {
    public string Sql => @"
        CREATE TABLE SavedMovies (
            [UserId] NVARCHAR(50) NOT NULL,
            [MovieId] INT NOT NULL,
            [IsWatched] BIT NOT NULL,
            CONSTRAINT FK_SavedMovies_UserId FOREIGN KEY ([UserId]) REFERENCES Users([Id]),
            CONSTRAINT PK_SavedMovies_UserId_MovieId PRIMARY KEY ([UserId], [MovieId])
        )
    "; 
}
