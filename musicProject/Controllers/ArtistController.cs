using Microsoft.AspNetCore.Mvc;
using musicProject.Models.ArtistModels;
using musicProject.Services.ArtistServices;

namespace musicProject.Controllers
{
    public class ArtistController : Controller
    {
        private IArtistService _artistService;
        public ArtistController(IArtistService artistService)
        {
            _artistService = artistService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var artists = await _artistService.GetAllArtistsAsync();
            return View(artists);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(string name)
        {
            var artist = await _artistService.GetArtistAsync(name);
            if (artist == null) return BadRequest();
            return View(artist);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ArtistCreate artistCreate = new ArtistCreate();
            return View(artistCreate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ArtistCreate model)
        {
            if (await _artistService.CreateArtistAsync(model))
                return RedirectToAction("Index");
            return View(model);
        }
    }
}
