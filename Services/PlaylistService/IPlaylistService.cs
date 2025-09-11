using PlaylistApi.DTOs.PlaylistDtos;
using PlaylistApi.Models;

namespace PlaylistApi.Services.PlaylistService
{
    public interface IPlaylistService
    {
        Task<IEnumerable<Playlist>> GetAllPlaylists();
        Task<IEnumerable<Playlist>> GetUserPlaylists(int userId);
        Task<Playlist?> GetPlaylistById(int id, int userId, bool isAdmin);
        Task<Playlist> CreatePlaylist(CreatePlaylistDto dto, int userId);
        Task<Playlist> UpdatePlaylist(UpdatePlaylistDto dto, int id, int userId, bool isAdmin);
        Task<Playlist> DeletePlaylist(int id, int userId, bool isAdmin);
    }
}
