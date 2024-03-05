using Microsoft.AspNetCore.Mvc;
using musicProject.Models.AlbumModels;
using musicProject.Services.AlbumServices;

namespace musicProject.Controllers
{
    public class AlbumController : Controller
    {
        private IAlbumService _albumService;
        public AlbumController(IAlbumService albumService)
        {
            _albumService = albumService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var albums = await _albumService.GetAllAlbumsAsync();
            return View(albums);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(string name)
        {
            var album = await _albumService.GetAlbumAsync(name);
            if (album == null) { return BadRequest(); }
            return View(album);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            AlbumCreate albumCreate = new AlbumCreate();
            return View(albumCreate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AlbumCreate model)
        {
            var albumId = await _albumService.CreateAlbumAsync(model);
            if (albumId > 0)
            {
                return RedirectToAction(("Track"), nameof(Create), new { id = albumId });
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AlbumsByArtist(string artistName)
        {
            var albums = await _albumService.GetAlbumsByArtistAsync(artistName);
            if (albums == null) return BadRequest();
            return View(albums);
        }
    }
}
