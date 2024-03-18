using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using musicProject.Data.Entities;
using musicProject.Models.AlbumReviewModels;
using musicProject.Models.TrackReviewModels;
using musicProject.Services.TrackReviewServices;
using musicProject.Services.TrackServices;

namespace musicProject.Controllers
{
    public class TrackReviewController : Controller
    {
        private ITrackReviewService _trackReviewService;
        private ITrackService _trackService;
        public TrackReviewController(ITrackReviewService trackReviewService, ITrackService trackService)
        {
            _trackReviewService = trackReviewService;
            _trackService = trackService;
        }

        [HttpGet]
        public async Task<IActionResult> NewIndex()
        {
            var reviews = await _trackService.GetTracksByAvgRatingAsync();
            return View(reviews);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["TrackId"] = new SelectList(await _trackService.GetAllTracksAsync(), "Id", "Title");

            TrackReviewCreate reviewCreate = new TrackReviewCreate();
            return View(reviewCreate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TrackId, Rating, Content, UserId")] TrackReviewCreate reviewCreate)
        {
            //ViewData["TrackId"] = new SelectList(await _trackService.GetAllTracksAsync(), "Id", "Title");
            if (await _trackReviewService.CreateTrackReviewAsync(reviewCreate))
                return RedirectToAction("NewIndex");
            return View(reviewCreate);
        }

        [HttpGet]
        public async Task<IActionResult> Review(int id)
        {
            var track = await _trackService.GetTrackByIdAsync(id);
            if (track == null) return BadRequest("Could not find track");
            TrackReviewCreate trackReview = new TrackReviewCreate();
            trackReview.TrackId = id;
            return View(trackReview);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Review(TrackReviewCreate trackReviewCreate)
        {
            if (await _trackReviewService.CreateTrackReviewAsync(trackReviewCreate))
                return RedirectToAction("NewIndex");
            return View(trackReviewCreate);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var review = await _trackReviewService.GetTrackReviewByIdAsync(id);
            if (review == null) return BadRequest("Could not find track review");
            return View(review);
        }

        [HttpGet]
        public async Task<IActionResult> ReviewsByTrack(int id)
        {
            var reviews = await _trackReviewService.GetReviewsByTrackIdAsync(id);
            if (reviews == null) return BadRequest("Could not find track");
            return View(reviews);
        }
        
        [HttpGet]
        public async Task<IActionResult> ReviewsByTrackArtist(string name)
        {
            var reviews = await _trackReviewService.GetReviewsByTrackArtistAsync(name);
            if (reviews == null) return BadRequest("Could not find track reviews");
            return View(reviews);
        }

        [HttpGet]
        public async Task<IActionResult> MyTrackReviews()
        {
            var reviews = await _trackReviewService.GetUserTrackReviewsAsync();
            if (reviews is null) return BadRequest("You haven't reviewed any tracks yet");
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
                return RedirectToAction("NewIndex");
            return View(reviewUpdate);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var review = await _trackReviewService.GetTrackReviewByIdAsync(id);
            if (review == null) return NotFound();
            return View(review);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteReview(int id)
        {
            if (await _trackReviewService.DeleteTrackReviewAsync(id)) return RedirectToAction("MyTrackReviews");
            return StatusCode(500, "Something went wrong. Please try again later");
        }

        // Unused methods

        //[HttpGet]
        //public async Task<IActionResult> ReviewsByTrack(string title)
        //{
        //    var reviews = await _trackReviewService.GetReviewsByTrackAsync(title);
        //    if (reviews == null) return BadRequest();
        //    return View(reviews);
        //}        
    }
}
