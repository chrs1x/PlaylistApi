using System.ComponentModel.DataAnnotations;

namespace PlaylistApi.DTOs.AuthDtos
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be 3-50 chars")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 chars")]
        public string Password { get; set; } = string.Empty;
    }
}
