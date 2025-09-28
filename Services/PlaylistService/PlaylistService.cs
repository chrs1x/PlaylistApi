using PlaylistApi.Data;
using PlaylistApi.DTOs.PlaylistDtos;
using PlaylistApi.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace PlaylistApi.Services.PlaylistService
{
    public class PlaylistService : IPlaylistService
    {
        private readonly AppDbContext _context;

        public PlaylistService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Playlist>> GetAllPlaylists() =>
            await _context.Playlists.ToListAsync();

        // User action
        public async Task<IEnumerable<Playlist>> GetUserPlaylists(int userId) =>
            await _context.Playlists
            .Where(p => p.UserId == userId)
            .ToListAsync();
        
        public async Task<Playlist?> GetPlaylistById(int id, int userId, bool isAdmin)
        {
            var playlist = await _context.Playlists.FindAsync(id);
            if (playlist == null) throw new KeyNotFoundException("Playlist not found.");

            if (playlist.UserId != userId && !isAdmin)
                throw new UnauthorizedAccessException("You aren't allowed to view this playlist.");

            return playlist;
        }

        public async Task<Playlist> CreatePlaylist(CreatePlaylistDto dto, int userId)
        {
            var playlist = new Playlist
            {
                Name = dto.Name,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };
            _context.Playlists.Add(playlist);
            await _context.SaveChangesAsync();
            return playlist;
        }

        public async Task<Playlist> UpdatePlaylist(UpdatePlaylistDto dto, int id, int userId, bool isAdmin)
        {
            var playlist = await _context.Playlists.FindAsync(id);
            if (playlist == null) throw new KeyNotFoundException("Playlist not found");

            
            if (playlist.UserId != userId && !isAdmin)
                throw new UnauthorizedAccessException("You are not allowed to update this playlist.");

            if (!string.IsNullOrWhiteSpace(dto.Name))
                playlist.Name = dto.Name;

            await _context.SaveChangesAsync();
            return playlist;
        }
        public async Task<Playlist> DeletePlaylist(int id, int userId, bool isAdmin)
        {
            var playlist = await _context.Playlists.FindAsync(id);
            if (playlist == null) throw new KeyNotFoundException("Playlist not found");

            if (playlist.UserId != userId && !isAdmin)
                throw new UnauthorizedAccessException("You are not allowed to delete this playlist.");

            _context.Playlists.Remove(playlist);
            await _context.SaveChangesAsync();
            return playlist;
        }
    }
}
