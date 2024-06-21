using Microsoft.EntityFrameworkCore;
using SongService.Entity;
using System.Data.Common;

namespace SongService.Repository;

public class SongContext : DbContext
{
    public DbSet<Song> Songs { get; set; }

    public string? ConnectionString { get; }

    // Builds a connection string from environment variables
    public SongContext(IReadOnlyDictionary<string, string> envStore, ILogger<SongContext> logger)
    {
        var builder = new DbConnectionStringBuilder
        {
            { "HOST", envStore["DB_HOST"] },
            { "PORT", envStore["DB_PORT"] },
            { "USERNAME", envStore["DB_USERNAME"] },
            { "PASSWORD", envStore["DB_PASSWORD"] },
            { "DATABASE", envStore["DB_DATABASE"] }
        };

        ConnectionString = builder.ConnectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options) =>
        options.UseNpgsql(ConnectionString);
}