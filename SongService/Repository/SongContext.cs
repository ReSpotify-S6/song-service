using Microsoft.EntityFrameworkCore;
using SongService.Entity;
using System.Data.Common;

namespace SongService.Repository;

public class SongContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Song> Songs { get; set; }
}