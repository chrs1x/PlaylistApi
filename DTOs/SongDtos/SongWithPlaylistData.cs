namespace Playlist_API.DTOs.SongDtos
{
    public class SongWithPlaylistData
    {
        public int SongId { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public TimeSpan? Duration { get; set; }

        // Playlist Specific
        public DateTime AddedAt { get; set; }
        public int Order { get; set; }
        public int PlayCount { get; set; }
        public string? Notes { get; set; }
    }
}
