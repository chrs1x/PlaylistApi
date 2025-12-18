using PlaylistApi.Models;

namespace PlaylistApi.Models
{
    public class PlaylistSong
    {
        public int Id { get; set; }
        public int PlaylistId { get; set; }
        public Playlist Playlist { get; set; }

        public int SongId { get; set; }
        public Song Song { get; set; }

        // Playlist Specific
        public DateTime AddedAt { get; set; }
        public int Order { get; set; }
        public int PlayCount { get; set; } = 0;
        public string? Notes { get; set; }
    }
}
