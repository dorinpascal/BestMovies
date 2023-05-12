using Migr8;

namespace BestMovies.Api.Migrations;

[Migration(3, "Create Reviews Table")]
public class _002_AddReviewTable : ISqlMigration
{
    public string Sql => @"
         create table Reviews (
            [id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
            [userId] VARCHAR(50) NOT NULL,
            [rating] VARCHAR(50) NOT NULL,
			[comment] VARCHAR (255), 
            FOREIGN KEY (userId) REFERENCES Users(id)
        )
    ";
}
