using System.ComponentModel.DataAnnotations;

namespace PlaylistApi.DTOs.SongDtos
{
    public class CreateSongDto
    {
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Artist { get; set; }
        public TimeSpan? Duration { get; set; }
    }
}
