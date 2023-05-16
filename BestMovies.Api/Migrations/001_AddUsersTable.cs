using Migr8;

namespace BestMovies.Api.Migrations;

[Migration(1, "Create Users Table")]
public class _001_AddUsersTable : ISqlMigration
 {
     public string Sql => @"
        CREATE TABLE Users (
            [Id] NVARCHAR(50) NOT NULL,
            [Email] NVARCHAR(125) NOT NULL,
            
            CONSTRAINT PK_UserId PRIMARY KEY ([Id])
        )
    "; 
 }