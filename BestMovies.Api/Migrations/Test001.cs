using Migr8;

namespace BestMovies.Api.Migrations;


[Migration(1, "Create test table")]
public class Test001 : ISqlMigration
{
    public string Sql => @"
		create table Test (
		    [Id] INT IDENTITY(1,1),
		    [Message] varchar(250) NULL,
		    
		    primary key ([Id])
		)
	"; 
}