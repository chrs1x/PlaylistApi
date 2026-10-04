using System.ComponentModel.DataAnnotations;

namespace PlaylistApi.DTOs.PlaylistDtos
{
    public class CreatePlaylistDto
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name must be 100 characters or fewer")]
        public string Name { get; set; } = string.Empty;
    }
}
