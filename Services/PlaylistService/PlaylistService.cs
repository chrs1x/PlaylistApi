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
        public async Task<IEnumerable<Playlist>> GetUserPlaylists(int userId);
        public async Task<Playlist?> GetPlaylistById(int id) =>
            await _context.Playlists.FindAsync(id);
        public async Task<Playlist> CreatePlaylist(CreatePlaylistDto dto, int userId)
        {

        }
        public async Task<Playlist> UpdatePlaylist(UpdatePlaylistDto dto, int id, int userId, bool isAdmin)
        {

        }
        public async Task<Playlist> DeletePlaylist(int id, int userId, bool isAdmin)
        {

        }
    }
}
