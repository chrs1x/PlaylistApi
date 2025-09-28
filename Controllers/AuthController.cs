using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlaylistApi.DTOs.AuthDtos;
using PlaylistApi.Services.Auth;

namespace PlaylistApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<string>> Register(RegisterDto dto)
        {
            try
            {
                var token = await _authService.Register(dto);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginDto dto)
        {
            try
            {
                var token = await _authService.Login(dto);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }
    }
}
