using Microsoft.EntityFrameworkCore;
using SongService.Entity;
using System.Configuration;
using System.Data.Common;

namespace SongService.Repository;

public class SongContext : DbContext
{
    public DbSet<Song> Songs { get; set; }

    public string? ConnectionString { get; }

    // Builds a connection string from environment variables
    public SongContext(IConfiguration configuration, ILogger<SongContext> logger)
    {
        ConnectionString = configuration.GetConnectionString("DefaultConnection");

        var builder = new DbConnectionStringBuilder();
        var envVars = new List<string>()
        {
            "DB_HOST", "DB_PORT", "DB_USERNAME", "DB_PASSWORD", "DB_DATABASE"
        };
        foreach (var envVar in envVars)
        {
            string? value = Environment.GetEnvironmentVariable(envVar); 

            if (value is null)
            {
                logger.LogWarning("Could not construct database connection string. Environment variable '{}' is not set.", envVar);
                logger.LogWarning("Falling back to configuration default: '{}'", ConnectionString);
                return;
            }

            builder.Add(envVar[3..], value);
        }
        logger.LogInformation(builder.ConnectionString);
        ConnectionString = builder.ConnectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options) =>
        options.UseNpgsql(ConnectionString);
}