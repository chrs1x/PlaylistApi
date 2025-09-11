using PlaylistApi.Data;

namespace PlaylistApi.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly JwtTokenGenerator _tokenGenerator;

        public AuthService(AppDbContext context)
        {
            _context = context;
        }
    }
