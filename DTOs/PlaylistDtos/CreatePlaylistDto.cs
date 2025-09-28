using System.ComponentModel.DataAnnotations;

namespace PlaylistApi.DTOs.PlaylistDtos
{
    public class CreatePlaylistDto
    {
        [Required]
        public string Name { get; set; }
    }
}
