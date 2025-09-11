namespace PlaylistApi.DTOs.SongDtos
{
    public class UpdateSongDto
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public TimeSpan? Duration { get; set; }
    }
}
