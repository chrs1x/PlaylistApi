using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlaylistApi.DTOs.PlaylistDtos;
using PlaylistApi.Models;
using PlaylistApi.Services.PlaylistService;
using PlaylistApi.Services.SongService;
using System.Security.Claims;

namespace PlaylistApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PlaylistsController : ControllerBase
    {
        private readonly IPlaylistService _playlistService;

        public PlaylistsController(IPlaylistService playlistService)
        {
            _playlistService = playlistService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<Playlist>>> GetAllPlaylists() => Ok(await _playlistService.GetAllPlaylists());

        [HttpGet("{id}")]
        public async Task<ActionResult<Playlist?>> GetPlaylistById(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var isAdmin = User.IsInRole("Admin");

            var playlist = await _playlistService.GetPlaylistById(id, userId, isAdmin);
            return playlist == null ? NotFound() : Ok(playlist);
        }

        [HttpPost]
        public async Task<ActionResult<Playlist>> CreatePlaylist(CreatePlaylistDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (!ModelState.IsValid) return BadRequest(ModelState);
            var playlist = await _playlistService.CreatePlaylist(dto, userId);
            return CreatedAtAction(nameof(GetPlaylistById), new { id = playlist.Id }, playlist);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<Playlist>> UpdatePlaylist(UpdatePlaylistDto dto, int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var isAdmin = User.IsInRole("Admin");

            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var updatedPlaylist = await _playlistService.UpdatePlaylist(dto, id, userId, isAdmin);
                return Ok(updatedPlaylist);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Playlist>> DeletePlaylist(int id)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var isAdmin = User.IsInRole("Admin");

                var deleted = await _playlistService.DeletePlaylist(id, userId, isAdmin);
                return Ok(deleted);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
        }

        [HttpGet("user")]
        public async Task<ActionResult<IEnumerable<Playlist>>> GetUserPlaylists()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return Ok(await _playlistService.GetUserPlaylists(userId));
        }

            
    }
}
