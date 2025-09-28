using Microsoft.EntityFrameworkCore;
using PlaylistApi.Data;
using PlaylistApi.DTOs.AuthDtos;
using PlaylistApi.Models;
using PlaylistApi.Utils;
using System.Security.Cryptography;
using System.Text;

namespace PlaylistApi.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly JwtTokenGenerator _tokenGenerator;

        public AuthService(AppDbContext context, JwtTokenGenerator tokenGenerator)
        {
            _context = context;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<string> Register(RegisterDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
                throw new Exception("Username already exists");

            CreatePasswordHash(dto.Password, out byte[] hash, out byte[] key); // hash password

            var user = new User
            {
                Username = dto.Username,
                PasswordHash = hash,
                PasswordKey = key,
                Role = "User" // default role
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return _tokenGenerator.GenerateToken(user); // generate token 
        }
        public async Task<string> Login(LoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
            if (user == null) throw new Exception("Invalid credentials");

            if (!VerifyPassword(dto.Password, user.PasswordHash, user.PasswordKey))
                throw new Exception("Invalid credentials");

            return _tokenGenerator.GenerateToken(user);
        }

        private void CreatePasswordHash(string password, out byte[] hash, out byte[] key)
        {
            using var hmac = new HMACSHA512(); // makes a hash auth code
            key = hmac.Key; // each user has their own random key stored in the database
            hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)); // uses the key to produce a hashed password
        }

        private bool VerifyPassword(string password, byte[] hash, byte[] key)
        {
            using var hmac = new HMACSHA512(key); // uses the stored key for this user
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)); // converts typed password into a hashed password
            return computedHash.SequenceEqual(hash); // checks if it matches the hashed pass stored in the database
        }
    }
}
