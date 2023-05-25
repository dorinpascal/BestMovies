namespace BestMovies.Api.Migrations;

using Migr8;


[Migration(4, "Atler Users table to make email column unique")]
public class _004_UniqueEmailOnUserTable {
    public string Sql => @"
        ALTER TABLE [Users]
        ADD CONSTRAINT UQ_Users_Email UNIQUE(Email);

    "; 
}