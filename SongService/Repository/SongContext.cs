using Microsoft.EntityFrameworkCore;
using SongService.Entity;
using System.Configuration;

namespace SongService.Repository;

public class SongContext : DbContext
{
    public DbSet<Song> Songs { get; set; }

    public string? ConnectionString { get; }

    public SongContext(IConfiguration configuration) 
    {
        ConnectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING") 
            ?? configuration.GetConnectionString("DefaultConnection");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options) =>
        options.UseNpgsql(ConnectionString);
}