using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using musicProject.Data.Data;
using musicProject.Data.Entities;
using musicProject.Models.AlbumModels;
using musicProject.Services.AlbumServices;
using musicProject.Services.ArtistServices;

namespace musicProject.Controllers
{
    public class AlbumController : Controller
    {
        private IAlbumService _albumService;
        private IArtistService _artistService;
        //private readonly ApplicationDbContext _context;
        public AlbumController(IAlbumService albumService, /*ApplicationDbContext context,*/ IArtistService artistService)
        {
            _albumService = albumService;
            //_context = context;
            _artistService = artistService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var albums = await _albumService.GetAllAlbumsAsync();
            return View(albums);
        }

        [HttpGet]
        public async Task<IActionResult> DetailByName(string name)
        {
            var album = await _albumService.GetAlbumAsync(name);
            if (album == null) { return BadRequest(); }
            return View(album);
        }       
        
        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var album = await _albumService.GetAlbumByIdAsync(id);
            if (album == null) { return BadRequest(); }
            return View(album);
        }        
        
        [HttpGet]
        public async Task<IActionResult> DetailFromIndex(int id)
        {
            var album = await _albumService.GetAlbumByIdAsync(id);
            if (album == null) { return BadRequest(); }
            return RedirectToAction("Detail", "Album", new { albumId = id });
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["ArtistId"] = new SelectList(await _artistService.GetAllArtistsAsync(), "Id", "Name");
            AlbumCreate albumCreate = new AlbumCreate();
            return View(albumCreate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ArtistId,Title,Genre,Released")] AlbumCreate model)
        {
            var albumId = await _albumService.CreateAlbumAsync(model);
            if (albumId > 0)
            {
                return RedirectToAction("Create", "Track", new { albumId = albumId });
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
