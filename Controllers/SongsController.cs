using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlaylistApi.DTOs.SongDtos;
using PlaylistApi.Models;
using PlaylistApi.Services.SongService;

namespace PlaylistApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongsController : ControllerBase
    {
        private readonly ISongService _songService;

        public SongsController(ISongService songService)
        {
            _songService = songService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Song>>> GetAllSongs() => Ok(await _songService.GetAllSongs());

        [HttpGet("{id}")]
        public async Task<ActionResult<Song?>> GetSongById(int id)
        {
            var song = await _songService.GetSongById(id);
            return song == null ? NotFound() : Ok(song);
        }

        [HttpPost]
        public async Task<ActionResult<Song>> CreateSong([FromBody] CreateSongDto dto)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);
            var song = await _songService.CreateSong(dto);
            return CreatedAtAction(nameof(GetSongById), new { id = song.Id }, song);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<Song>> UpdateSong(int id, [FromBody] UpdateSongDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var updatedSong = await _songService.UpdateSong(id, dto);
                return Ok(updatedSong);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Song>> DeleteSong(int id)
        {
            var success = await _songService.DeleteSong(id);
            return success == null ? NotFound() : Ok(success);
        }

        // User actions

        [HttpGet("~/api/playlists/{playlistId}/songs")]
        public async Task<ActionResult<IEnumerable<Song>>> GetSongsForPlaylist(int playlistId) 
            => Ok(await _songService.GetSongsForPlaylist(playlistId));

        [HttpPost("/api/playlists/{playlistId}/songs/{songId}")]
        public async Task<ActionResult<Song>> AddSongToPlaylist(int playlistId, int songId)
        {
            var song = await _songService.AddSongToPlaylist(playlistId, songId);
            return Ok(song);
        }

        [HttpDelete("/api/playlists/{playlistId}/songs/{songId}")]
        public async Task<ActionResult<Song>> RemoveSongFromPlaylist(int playlistId, int songId)
        {
            var success = await _songService.RemoveSongFromPlaylist(playlistId, songId);
            return success == null ? NotFound() : Ok(success);
        }
    }
}
