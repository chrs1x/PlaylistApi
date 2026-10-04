using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlaylistApi.DTOs.PlaylistDtos;
using PlaylistApi.Models;
using PlaylistApi.Services.PlaylistService;

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

        private int CurrentUserId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        private bool IsAdmin => User.IsInRole("Admin");

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<Playlist>>> GetAllPlaylists() =>
            Ok(await _playlistService.GetAllPlaylists());

        [HttpGet("user")]
        public async Task<ActionResult<IEnumerable<Playlist>>> GetUserPlaylists() =>
            Ok(await _playlistService.GetUserPlaylists(CurrentUserId));

        [HttpGet("{id}")]
        public async Task<ActionResult<Playlist>> GetPlaylistById(int id)
        {
            try
            {
                return Ok(await _playlistService.GetPlaylistById(id, CurrentUserId, IsAdmin));
            }
            catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
            catch (UnauthorizedAccessException ex) { return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message }); }
        }

        [HttpPost]
        public async Task<ActionResult<Playlist>> CreatePlaylist(CreatePlaylistDto dto)
        {
            var playlist = await _playlistService.CreatePlaylist(dto, CurrentUserId);
            return CreatedAtAction(nameof(GetPlaylistById), new { id = playlist.Id }, playlist);
        }
      
        [HttpPatch("{id}")]
        public async Task<ActionResult<Playlist>> UpdatePlaylist(int id, UpdatePlaylistDto dto)
        {
            try
            {
                return Ok(await _playlistService.UpdatePlaylist(dto, id, CurrentUserId, IsAdmin));
            }
            catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
            catch (UnauthorizedAccessException ex) { return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message }); }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Playlist>> DeletePlaylist(int id)
        {
            try
            {
                return Ok(await _playlistService.DeletePlaylist(id, CurrentUserId, IsAdmin));
            }
            catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
            catch (UnauthorizedAccessException ex) { return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message }); }
        }
    }
}