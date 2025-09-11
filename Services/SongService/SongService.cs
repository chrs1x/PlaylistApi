using PlaylistApi.Data;
using PlaylistApi.DTOs.SongDtos;
using PlaylistApi.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace PlaylistApi.Services.SongService
{
    public class SongService : ISongService
    {
        private readonly AppDbContext _context;

        public SongService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Song>> GetAllSongs() =>
            await _context.Songs.ToListAsync();

        public async Task<Song?> GetSongById(int id) =>
            await _context.Songs.FindAsync(id);
        public async Task<Song> CreateSong(CreateSongDto dto)
        {
            var song = new Song
            {
                Title = dto.Title,
                Artist = dto.Artist,
                Duration = dto.Duration
            };
            _context.Songs.Add(song);
            await _context.SaveChangesAsync();
            return song;
        }
        public async Task<Song> UpdateSong(int id, UpdateSongDto dto)
        {
            var song = await _context.Songs.FindAsync(id);
            if (song == null) throw new KeyNotFoundException($"Song not found");

            if (!string.IsNullOrWhiteSpace(dto.Title)) 
                song.Title = dto.Title;
            if (!string.IsNullOrWhiteSpace(dto.Artist)) 
                song.Artist = dto.Artist; 
            if (dto.Duration.HasValue)
                song.Duration = dto.Duration;

            await _context.SaveChangesAsync();
            return song;
        }

        public async Task<Song> DeleteSong(int id)
        {
            var song = await _context.Songs.FindAsync(id);
            if (song == null) return null;
            _context.Songs.Remove(song);
            await _context.SaveChangesAsync();
            return song;
        }

        // User Actions
        public async Task<IEnumerable<Song>> GetSongsForPlaylist(int playlistId)
        {
            var playlist = await _context.Playlists
                .Include(p => p.Songs)
                .FirstOrDefaultAsync(p => p.Id == playlistId);

            if (playlist == null) throw new Exception("Playlist not found.");

            return playlist.Songs;
            
        }
        public async Task<Song> AddSongToPlaylist(int playlistId, int songId)
        {
            var playlist = await _context.Playlists
                .Include(p => p.Songs)
                .FirstOrDefaultAsync(p => p.Id == playlistId);

            if (playlist == null) throw new Exception("Playlist not found.");

            var song = await _context.Songs.FindAsync(songId);
            if (song == null) throw new KeyNotFoundException("Song not found.");

            if(!playlist.Songs.Any(s => s.Id == songId))
            {
                playlist.Songs.Add(song);
                await _context.SaveChangesAsync();
            }
            return song;
        }
        public async Task<Song> RemoveSongFromPlaylist(int playlistId, int songId)
        {
            var playlist = await _context.Playlists
                .Include(p => p.Songs)
                .FirstOrDefaultAsync(p => p.Id == playlistId);

            if (playlist == null) throw new Exception("Playlist not found.");

            var song = await _context.Songs.FindAsync(songId);
            if (song == null) throw new KeyNotFoundException("Song not found.");

            playlist.Songs.Remove(song);
            await _context.SaveChangesAsync();

            return song;
        }
    }
}
