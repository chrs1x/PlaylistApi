using Microsoft.EntityFrameworkCore;
using PlaylistApi.Models;
namespace PlaylistApi.Data
{
    public class AppDbContext : DbContext
    {
        private readonly AppDbContext _context;

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<Song> Songs { get; set; }
    }
}
