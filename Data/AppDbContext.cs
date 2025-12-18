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
        public DbSet<PlaylistSong> PlaylistSongs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // creates composite unique key for song & playlist
            modelBuilder.Entity<PlaylistSong>()
                .HasIndex(ps => new { ps.PlaylistId, ps.SongId })
                .IsUnique(); // ensures you cant have 2 of the same song in 1 playlist
        }
    }
}
