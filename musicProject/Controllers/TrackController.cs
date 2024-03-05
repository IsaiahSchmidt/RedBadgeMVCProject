using Microsoft.AspNetCore.Mvc;
using musicProject.Models.TrackModels;
using musicProject.Services.AlbumServices;
using musicProject.Services.TrackServices;

namespace musicProject.Controllers
{
    public class TrackController : Controller
    {
        private ITrackService _trackService;
        private IAlbumService _albumService;
        public TrackController(ITrackService trackService, IAlbumService albumService)
        {
            _trackService = trackService;
            _albumService = albumService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var tracks = await _trackService.GetAllTracksAsync();
            return View(tracks);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(string title)
        {
            var track = await _trackService.GetTrackAsync(title);
            if (track == null) { return BadRequest(); }
            return View(track);
        }
        [HttpGet]
        public async Task<IActionResult> TrackByArtist(string artistName)
        {
            var track = await _trackService.GetTracksByArtistAsync(artistName);
            if (track == null) { return BadRequest(); }
            return View(track);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int albumId)
        {
            var album = await _albumService.GetAlbumByIdAsync(albumId);
            if (album == null) return BadRequest();
            //make 404 if time
            TrackCreate trackCreate = new TrackCreate();
            trackCreate.AlbumId = albumId;
            return View(trackCreate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TrackCreate model)
        {
            if (await _trackService.CreateTrackAsync(model))
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}
