using System.ComponentModel.DataAnnotations;

namespace PlaylistApi.DTOs.PlaylistDtos
{
    public class UpdatePlaylistDto
    {
        [StringLength(100, ErrorMessage = "Name must be 100 characters or fewer")]
        public string? Name { get; set; }
    }
}
