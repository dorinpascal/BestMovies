using Migr8;

namespace BestMovies.Api.Migrations;

[Migration(2, "Create Reviews Table")]
public class _002_AddReviewTable : ISqlMigration
{
    public string Sql => @"
         CREATE TABLE Reviews (
            [UserId] NVARCHAR(50) NOT NULL,
            [MovieId] INT NOT NULL,
            [Rating] INT NOT NULL,
			[Comment] TEXT NULL, 
             
            CONSTRAINT PK_Reviews_UserId_MovieId PRIMARY KEY ([UserId], [MovieId]),
            CONSTRAINT FK_Reviews_UserId FOREIGN KEY ([UserId]) REFERENCES Users([Id])
        )
    ";
}
