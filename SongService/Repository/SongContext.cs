using Microsoft.EntityFrameworkCore;
using SongService.Entity;
using System.Data.Common;

namespace SongService.Repository;

public class SongContext : DbContext
{
    public DbSet<Song> Songs { get; set; }

    public string? ConnectionString { get; }

    // Builds a connection string from environment variables
    public SongContext(EnvironmentVariableManager envManager, ILogger<SongContext> logger)
    {
        var builder = new DbConnectionStringBuilder
        {
            { "HOST", envManager["DB_HOST"] },
            { "PORT", envManager["DB_PORT"] },
            { "USERNAME", envManager["DB_USERNAME"] },
            { "PASSWORD", envManager["DB_PASSWORD"] },
            { "DATABASE", envManager["DB_DATABASE"] }
        };

        ConnectionString = builder.ConnectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options) =>
        options.UseNpgsql(ConnectionString);
}