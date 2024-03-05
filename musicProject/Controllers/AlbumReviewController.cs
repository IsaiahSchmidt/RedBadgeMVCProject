using Microsoft.AspNetCore.Mvc;
using musicProject.Models.AlbumReviewModels;
using musicProject.Services.AlbumReviewServices;

namespace musicProject.Controllers
{
    public class AlbumReviewController : Controller
    {
        private IAlbumReviewService _albumReviewService;
        public AlbumReviewController(IAlbumReviewService albumReviewService)
        {
            _albumReviewService = albumReviewService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var reviews = await _albumReviewService.GetUserAlbumReviewsAsync();
            return View(reviews);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            AlbumReviewCreate reviewCreate = new AlbumReviewCreate();
            return View(reviewCreate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AlbumReviewCreate reviewCreate)
        {
            if (await _albumReviewService.CreateAlbumReviewAsync(reviewCreate))
                return RedirectToAction("Index");
            return View(reviewCreate);
        }

        [HttpGet]
        public async Task<IActionResult> ReviewsByAlbum(string title)
        {
            var reviews = await _albumReviewService.GetAlbumReviewsByAlbumAsync(title);
            if (reviews == null) return BadRequest();
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
                return RedirectToAction("Index");
            return View(reviewUpdate);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var review = await _albumReviewService.GetAlbumReviewByIdAsync(id);
            if (review == null) { return NotFound(); };
            return View(review);
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteReview(int id)
        {
            if (await _albumReviewService.DeleteAlbumReviewAsync(id)) return RedirectToAction("Index");
            return StatusCode(500, "Internal server error :( ");
        }
    }
}
