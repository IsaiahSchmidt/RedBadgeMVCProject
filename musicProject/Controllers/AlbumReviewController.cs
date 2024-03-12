using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using musicProject.Data.Data;
using musicProject.Models.AlbumReviewModels;
using musicProject.Services.AlbumReviewServices;
using musicProject.Services.AlbumServices;

namespace musicProject.Controllers
{
    public class AlbumReviewController : Controller
    {
        private IAlbumReviewService _albumReviewService;
        private IAlbumService _albumService;
        //private readonly ApplicationDbContext _context;
        public AlbumReviewController(IAlbumReviewService albumReviewService /*ApplicationDbContext context*/, IAlbumService albumService)
        {
            _albumReviewService = albumReviewService;
            _albumService = albumService;
            //_context = context;
        }

        [HttpGet]
        public async Task<IActionResult> NewIndex()
        {
            var reviews = await _albumService.GetAlbumsByAvgRatingAsync();
            return View(reviews);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["AlbumId"] = new SelectList(await _albumService.GetAllAlbumsAsync(), "Id", "Title");
            AlbumReviewCreate reviewCreate = new AlbumReviewCreate();
            return View(reviewCreate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AlbumId, Rating, Content, UserId")] AlbumReviewCreate reviewCreate)
        {
            if (await _albumReviewService.CreateAlbumReviewAsync(reviewCreate))
                return RedirectToAction("NewIndex");
            return View(reviewCreate);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var review = await _albumReviewService.GetAlbumReviewByIdAsync(id);
            if (review == null) return BadRequest();
            return View(review);
        }

        [HttpGet]
        public async Task<IActionResult> ReviewsByAlbum(int id)
        {
            var reviews = await _albumReviewService.GetReviewsByAlbumIdAsync(id);
            if(reviews == null) return BadRequest();
            return View(reviews);
        }

        //[HttpGet]
        //public async Task<IActionResult> ReviewsByAlbum(string title)
        //{
        //    var reviews = await _albumReviewService.GetAlbumReviewsByAlbumAsync(title);
        //    if (reviews == null) return BadRequest();
        //    return View(reviews);
        //}        
        
        [HttpGet]
        public async Task<IActionResult> ReviewsByArtist(string name)
        {
            var reviews = await _albumReviewService.GetAlbumReviewsByArtistAsync(name);
            if (reviews == null) return BadRequest();
            return View(reviews);
        }

        [HttpGet]
        public async Task<IActionResult> MyAlbumReviews()
        {
            var reviews = await _albumReviewService.GetUserAlbumReviewsAsync();
            if (reviews is null) return BadRequest();
            return View(reviews);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var review = await _albumReviewService.GetAlbumReviewByIdAsync(id);
            if (review == null) { return NotFound(); }
            AlbumReviewUpdate reviewUpdate = new AlbumReviewUpdate()
            {
                Id = review.Id,
                Content = review.Content,
                Rating = review.Rating
            };
            return View(reviewUpdate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(AlbumReviewUpdate reviewUpdate)
        {
            if (!ModelState.IsValid) return View(ModelState);
            if (await _albumReviewService.UpdateAlbumReviewAsync(reviewUpdate))
                return RedirectToAction("NewIndex");
            return View(reviewUpdate);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var review = await _albumReviewService.GetAlbumReviewByIdAsync(id);
            if (review == null) return NotFound();
            return View(review);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteReview(int id)
        {
            if (await _albumReviewService.DeleteAlbumReviewAsync(id)) return RedirectToAction("MyAlbumReviews");
            return StatusCode(500, "Something went wrong. Please try again later");
        }
    }
}
