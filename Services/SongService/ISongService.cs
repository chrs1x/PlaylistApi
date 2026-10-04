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
        Task<IEnumerable<SongWithPlaylistData>> GetSongsForPlaylist(int playlistId, int userId, bool isAdmin);
        Task<PlaylistSong> AddSongToPlaylist(int playlistId, int songId, int userId, bool isAdmin);
        Task<PlaylistSong> RemoveSongFromPlaylist(int playlistId, int songId, int userId, bool isAdmin);

    }
}
