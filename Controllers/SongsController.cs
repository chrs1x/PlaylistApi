using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlaylistApi.DTOs.SongDtos;
using PlaylistApi.Models;
using PlaylistApi.Services.SongService;
using System.Security.Claims;

namespace PlaylistApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SongsController : ControllerBase
    {
        private readonly ISongService _songService;

        public SongsController(ISongService songService)
        {
            _songService = songService;
        }

        private int CurrentUserId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        private bool IsAdmin => User.IsInRole("Admin");

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Song>>> GetAllSongs() => Ok(await _songService.GetAllSongs());

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Song>> GetSongById(int id)
        {
            var song = await _songService.GetSongById(id);
            return song == null ? NotFound() : Ok(song);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Song>> CreateSong([FromBody] CreateSongDto dto)
        {
            var song = await _songService.CreateSong(dto);
            return CreatedAtAction(nameof(GetSongById), new { id = song.Id }, song);
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Song>> UpdateSong(int id, [FromBody] UpdateSongDto dto)
        {
            try
            {
                return Ok(await _songService.UpdateSong(id, dto));
            }
            catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Song>> DeleteSong(int id)
        {
            var deleted = await _songService.DeleteSong(id);
            return deleted == null ? NotFound() : Ok(deleted);
        }

        // User actions

        [HttpGet("/api/playlists/{playlistId}/songs")]
        public async Task<ActionResult<IEnumerable<SongWithPlaylistData>>> GetSongsForPlaylist(int playlistId)
        {
            try
            {
                return Ok(await _songService.GetSongsForPlaylist(playlistId, CurrentUserId, IsAdmin));
            }
            catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
            catch (UnauthorizedAccessException ex) { return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message }); }
        }

        [HttpPost("/api/playlists/{playlistId}/songs/{songId}")]
        public async Task<ActionResult<PlaylistSong>> AddSongToPlaylist(int playlistId, int songId)
        {
            try
            {
                return Ok(await _songService.AddSongToPlaylist(playlistId, songId, CurrentUserId, IsAdmin));
            }
            catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
            catch (UnauthorizedAccessException ex) { return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message }); }
            catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); } // 409: already in playlist
        }

        [HttpDelete("/api/playlists/{playlistId}/songs/{songId}")]
        public async Task<ActionResult<PlaylistSong>> RemoveSongFromPlaylist(int playlistId, int songId)
        {
            try
            {
                return Ok(await _songService.RemoveSongFromPlaylist(playlistId, songId, CurrentUserId, IsAdmin));
            }
            catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
            catch (UnauthorizedAccessException ex) { return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message }); }
        }
    }
}
