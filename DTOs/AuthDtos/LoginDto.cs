using System.ComponentModel.DataAnnotations;

namespace PlaylistApi.DTOs.AuthDtos
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string Password { get; set; }
    }
}
