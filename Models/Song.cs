namespace PlaylistApi.Models
{
    public class Song
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public TimeSpan? Duration { get; set; }

        public ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();
    }
}
