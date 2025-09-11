namespace PlaylistApi.DTOs.SongDtos
{
    public class CreateSongDto
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public TimeSpan? Duration { get; set; }
    }
}
