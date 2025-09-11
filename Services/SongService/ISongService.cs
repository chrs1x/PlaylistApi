using PlaylistApi.Models;
using PlaylistApi.DTOs.SongDtos;
namespace PlaylistApi.Services.SongService
{
    public interface ISongService
    {
        // Admin only
        Task<IEnumerable<Song>> GetAllSongs();
        Task<Song?> GetSongById(int id);
        Task<Song> CreateSong(CreateSongDto dto);
        Task<Song> UpdateSong(int id, UpdateSongDto dto);
        Task<Song> DeleteSong(int id);

        // User actions
        Task<IEnumerable<Song>> GetSongsForPlaylist(int playlistId);
        Task<Song> AddSongToPlaylist(int playlistId, int songId);
        Task<Song> RemoveSongFromPlaylist(int playlistId, int songId);

    }
}
