using Microsoft.AspNetCore.Mvc;
using musicProject.Models.TrackReviewModels;
using musicProject.Services.TrackReviewServices;

namespace musicProject.Controllers
{
    public class TrackReviewController : Controller
    {
        private ITrackReviewService _trackReviewService;
        public TrackReviewController(ITrackReviewService trackReviewService)
        {
            _trackReviewService = trackReviewService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var reviews = await _trackReviewService.GetUserTrackReviewsAsync();
            return View(reviews);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            TrackReviewCreate reviewCreate = new TrackReviewCreate();
            return View(reviewCreate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TrackReviewCreate reviewCreate)
        {
            if (await _trackReviewService.CreateTrackReviewAsync(reviewCreate))
                return RedirectToAction("Index");
            return View(reviewCreate);
        }

        [HttpGet]
        public async Task<IActionResult> ReviewsByTrack(string title)
        {
            var reviews = await _trackReviewService.GetReviewsByTrackAsync(title);
            if (reviews == null) return BadRequest();
            return View(reviews);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var review = await _trackReviewService.GetTrackReviewByIdAsync(id);
            if (review == null) { return NotFound(); }
            TrackReviewUpdate reviewUpdate = new TrackReviewUpdate()
            {
                Id = review.Id,
                Content = review.Content,
                Rating = review.Rating
            };
            return View(reviewUpdate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(TrackReviewUpdate reviewUpdate)
        {
            if (!ModelState.IsValid) return View(ModelState);
            if (await _trackReviewService.UpdateTrackReviewAsync(reviewUpdate))
                return RedirectToAction("Index");
            return View(reviewUpdate);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var review = await _trackReviewService.GetTrackReviewByIdAsync(id);
            if (review == null) return NotFound();
            return View(review);
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteReview(int id)
        {
            if (await _trackReviewService.DeleteTrackReviewAsync(id)) return RedirectToAction("Index");
            return StatusCode(500, "Internal server error :( ");
        }
    }
}
