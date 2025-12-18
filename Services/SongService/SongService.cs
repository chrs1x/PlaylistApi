using Microsoft.EntityFrameworkCore;
using Playlist_API.DTOs.SongDtos;
using PlaylistApi.Services.SongService;
using PlaylistApi.Models;
using PlaylistApi.Data;
using PlaylistApi.DTOs.SongDtos;
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
        public async Task<IEnumerable<SongWithPlaylistData>> GetSongsForPlaylist(int playlistId)
        {
            var playlistSongs = await _context.PlaylistSongs
                .Where(ps => ps.PlaylistId == playlistId)
                .Include(ps => ps.Song)
                .OrderBy(ps => ps.Order)
                .Select(ps => new SongWithPlaylistData
                {
                    SongId = ps.Song.Id,
                    Title = ps.Song.Title,
                    Artist = ps.Song.Artist,
                    Duration = ps.Song.Duration,
                    AddedAt = ps.AddedAt,
                    Order = ps.Order,
                    PlayCount = ps.PlayCount,
                    Notes = ps.Notes
                })
                .ToListAsync();

            return playlistSongs;
            
        }
        public async Task<PlaylistSong> AddSongToPlaylist(int playlistId, int songId)
        {
            var playlist = await _context.Playlists
                .Include(p => p.PlaylistSongs)
                .FirstOrDefaultAsync(p => p.Id == playlistId);

            if (playlist == null) throw new Exception("Playlist not found.");

            var song = await _context.Songs.FindAsync(songId);
            if (song == null) throw new KeyNotFoundException("Song not found.");

            var exists = await _context.PlaylistSongs
                .AnyAsync(ps => ps.PlaylistId == playlistId && ps.SongId == songId);

            if (exists) throw new InvalidOperationException("Song is already in this playlist.");

            var maxOrder = await _context.PlaylistSongs
                .Where(ps => ps.PlaylistId == playlistId)
                .MaxAsync(ps => (int?)ps.Order) ?? 0; // if playlist is empty, max order = 0;

            var playlistSong = new PlaylistSong
            {
                PlaylistId = playlistId,
                SongId = songId,
                AddedAt = DateTime.UtcNow,
                Order = maxOrder + 1,
                PlayCount = 0,
                Notes = null
            };

            _context.PlaylistSongs.Add(playlistSong);
            await _context.SaveChangesAsync();

            return playlistSong;
        }
        public async Task<PlaylistSong> RemoveSongFromPlaylist(int playlistId, int songId)
        {
            var playlistSong = await _context.PlaylistSongs
                .FirstOrDefaultAsync(ps => ps.PlaylistId == playlistId && ps.SongId == songId);

            if (playlistSong == null)
                throw new KeyNotFoundException("Song not found in this playlist.");

            _context.PlaylistSongs.Remove(playlistSong);
            await _context.SaveChangesAsync();

            return playlistSong;
        }
    }
}
