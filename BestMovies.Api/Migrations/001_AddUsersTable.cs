using Migr8;

namespace BestMovies.Api.Migrations;

[Migration(1, "Create Users Table")]
public class _001_AddUsersTable : ISqlMigration
 {
     public string Sql => @"
        create table Users (
            [Id] VARCHAR(50) NOT NULL,
            [Email] VARCHAR(125) NOT NULL,
            
            primary key ([Id])
        )
    "; 
 }