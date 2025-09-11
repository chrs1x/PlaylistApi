using PlaylistApi.DTOs.Auth;
namespace PlaylistApi.Services.Auth
{
    public interface IAuthService
    {
        Task<string> Register(RegisterDto dto);
        Task<string> Login(LoginDto dto);
    }
}
